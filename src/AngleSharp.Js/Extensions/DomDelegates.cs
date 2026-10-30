namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    static class DomDelegates
    {
        private static readonly Type[] ToCallbackSignature = new[] { typeof(Function), typeof(EngineInstance) };
        private static readonly Type[] ToJsValueSignature = new[] { typeof(Object), typeof(EngineInstance) };

        //  Both of these depend on the delegate type alone, so they survive the conversion
        //  they were built for: turning a function into a delegate is otherwise a reflection
        //  lookup plus an expression tree compilation, every single time.
        private static readonly ConcurrentDictionary<Type, MethodInfo> _converters =
            new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly ConcurrentDictionary<Type, Delegate> _factories =
            new ConcurrentDictionary<Type, Delegate>();

        public static Delegate ToDelegate(this Type type, Function function, EngineInstance engine)
        {
            if (type != typeof(DomEventHandler))
            {
                var method = _converters.GetOrAdd(type, CreateConverter);
                return method.Invoke(null, new Object[] { function, engine }) as Delegate;
            }

            return function.ToListener(engine);
        }

        public static DomEventHandler ToListener(this Function function, EngineInstance engine) => (obj, ev) =>
        {
            var objAsJs = obj.ToJsValue(engine);
            var evAsJs = ev.ToJsValue(engine);

            try
            {
                function.Call(objAsJs, new[] { evAsJs });
            }
            catch (JavaScriptException jsException)
            {
                var window = (IWindow)engine.Window.Value;
                window.Fire<ErrorEvent>(e => e.Init(null, jsException.Location.Start.Line, jsException.Location.Start.Column, jsException));
            }
        };

        public static T ToCallback<T>(this Function function, EngineInstance engine)
        {
            var factory = (Func<Function, EngineInstance, T>)_factories.GetOrAdd(typeof(T), _ => CreateFactory<T>());
            return factory.Invoke(function, engine);
        }

        private static MethodInfo CreateConverter(Type type) =>
            typeof(DomDelegates).GetRuntimeMethod("ToCallback", ToCallbackSignature).MakeGenericMethod(type);

        private static Func<Function, EngineInstance, T> CreateFactory<T>()
        {
            var methodInfo = typeof(T).GetRuntimeMethods().First(m => m.Name == "Invoke");
            var convert = typeof(EngineExtensions).GetRuntimeMethod("ToJsValue", ToJsValueSignature);
            var mps = methodInfo.GetParameters();
            var parameters = new ParameterExpression[mps.Length];

            for (var i = 0; i < mps.Length; i++)
            {
                parameters[i] = Expression.Parameter(mps[i].ParameterType, mps[i].Name);
            }

            var objExpr = Expression.Parameter(typeof(Function), "function");
            var engineExpr = Expression.Parameter(typeof(EngineInstance), "engine");
            //  The first parameter is the receiver of the call, the others are its arguments; a
            //  delegate without parameters calls with no receiver. Value types (e.g., the time of
            //  an animation frame) have to be boxed for the conversion.
            Expression ToJsValue(ParameterExpression m) =>
                Expression.Call(convert, Expression.Convert(m, typeof(Object)), engineExpr);
            var call = Expression.Call(objExpr, "Call", new Type[0], new Expression[]
            {
                parameters.Length > 0 ? ToJsValue(parameters[0]) : Expression.Constant(JsValue.Undefined, typeof(JsValue)),
                Expression.NewArrayInit(typeof(JsValue), parameters.Skip(1).Select(ToJsValue).ToArray())
            });

            var callback = Expression.Lambda<T>(call, parameters);
            return Expression.Lambda<Func<Function, EngineInstance, T>>(callback, objExpr, engineExpr).Compile();
        }
    }
}
