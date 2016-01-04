namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint.Native;
    using Jint.Native.Function;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    static class DomDelegates
    {
        public static Delegate ToDelegate(this Type type, FunctionInstance function, EngineInstance engine)
        {
            if (type == typeof(DomEventHandler))
            {
                return function.ToListener(engine);
            }

            var method = typeof(DomDelegates).GetMethod("ToCallback").MakeGenericMethod(type);
            return method.Invoke(null, new Object[] { function, engine }) as Delegate;
        }

        public static DomEventHandler ToListener(this FunctionInstance function, EngineInstance engine)
        {
            return (obj, ev) =>
            {
                function.Call(obj.ToJsValue(engine), new [] { ev.ToJsValue(engine) });
            };
        }

        public static T ToCallback<T>(this FunctionInstance function, EngineInstance engine)
        {
            var type = typeof(T);
            var methodInfo = type.GetMethod("Invoke");
            var convert = typeof(Extensions).GetMethod("ToJsValue");
            var mps = methodInfo.GetParameters();
            var parameters = new ParameterExpression[mps.Length];

            for (var i = 0; i < mps.Length; i++)
            {
                parameters[i] = Expression.Parameter(mps[i].ParameterType, mps[i].Name);
            }

            var objExpr = Expression.Constant(function);
            var engineExpr = Expression.Constant(engine);
            var call = Expression.Call(objExpr, "Call", new Type[0], new Expression[]
            {
                Expression.Call(convert, parameters[0], engineExpr),
                Expression.NewArrayInit(typeof(JsValue), parameters.Skip(1).Select(m => Expression.Call(convert, m, engineExpr)).ToArray())
            });

            return Expression.Lambda<T>(call, parameters).Compile();
        }
    }
}
