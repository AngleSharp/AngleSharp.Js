namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

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

        //  What a function was turned into lives as long as the function, and doesn't keep it alive.
        private static readonly ConditionalWeakTable<JsValue, Function> _callables =
            new ConditionalWeakTable<JsValue, Function>();

        private static readonly ConditionalWeakTable<ObjectInstance, Dictionary<String, Function>> _callbackObjects = new ConditionalWeakTable<ObjectInstance, Dictionary<String, Function>>();
        private static readonly ConditionalWeakTable<Function, DomEventHandler> _listeners =
            new ConditionalWeakTable<Function, DomEventHandler>();

        /// <summary>
        /// Gets the function to call for what script gave as a callback, which can be anything
        /// callable: bound functions and proxies are no Function for Jint. Null if it's not callable.
        /// </summary>
        public static Function AsFunction(this JsValue value, EngineInstance engine)
        {
            if (value is Function function)
            {
                return function;
            }

            if (!value.IsCallable())
            {
                return null;
            }

            return _callables.GetValue(value, callable =>
            {
                var jint = engine.Jint;
                return new ClrFunction(jint, "callable", (self, arguments) => jint.Call(callable, self, arguments));
            });
        }

        /// <summary>
        /// Gets the function standing in for an object as a WebIDL callback interface (i.e. having
        /// handleEvent as a listener, or acceptNode as a filter), if the type is one. It asks the
        /// object for the method each time, as the standard says, and is the same one each time, so
        /// that a listener can be removed again.
        /// </summary>
        public static Function AsCallbackObject(this ObjectInstance target, Type type, EngineInstance engine)
        {
            var name = type == typeof(DomEventHandler) ? "handleEvent" : type == typeof(NodeFilter) ? "acceptNode" : null;

            if (name == null)
            {
                return null;
            }

            var functions = _callbackObjects.GetOrCreateValue(target);

            lock (functions)
            {
                if (!functions.TryGetValue(name, out var function))
                {
                    var jint = engine.Jint;
                    function = new ClrFunction(jint, name, (self, arguments) =>
                    {
                        var method = target.Get(name);

                        if (!method.IsCallable())
                        {
                            throw new JavaScriptException(jint.Intrinsics.TypeError, $"The object has no function {name}.");
                        }

                        return jint.Call(method, target, arguments);
                    });
                    functions[name] = function;
                }

                return function;
            }
        }

        public static Delegate ToDelegate(this Type type, Function function, EngineInstance engine)
        {
            if (type != typeof(DomEventHandler))
            {
                var method = _converters.GetOrAdd(type, CreateConverter);
                return method.Invoke(null, new Object[] { function, engine }) as Delegate;
            }

            return function.ToListener(engine);
        }

        /// <summary>
        /// Gets the listener of the function, which is the same one each time: this is how the
        /// listener added for a function is found again when script removes that function.
        /// </summary>
        public static DomEventHandler ToListener(this Function function, EngineInstance engine) =>
            _listeners.GetValue(function, key => key.CreateListener(engine));

        private static DomEventHandler CreateListener(this Function function, EngineInstance engine) => (obj, ev) =>
        {
            var objAsJs = obj.ToJsValue(engine);
            var evAsJs = ev.ToJsValue(engine);

            engine.EnterScript();

            try
            {
                function.Call(objAsJs, new[] { evAsJs });
            }
            catch (JavaScriptException jsException)
            {
                engine.ReportError(jsException);
            }
            finally
            {
                engine.LeaveScript();
            }
        };

        /// <summary>
        /// Gets what runs an event handler, i.e., an onclick and the like (HTML 8.1.8.1). Other than
        /// a listener, it can cancel the event by its result.
        /// </summary>
        public static DomEventHandler ToEventHandler(this Function function, EngineInstance engine) => (obj, ev) =>
        {
            var sender = obj.ToJsValue(engine);
            engine.EnterScript();

            try
            {
                //  the onerror of a window is the one handler not getting the event
                if (obj is IWindow && ev is ErrorEvent error)
                {
                    var handled = function.Call(sender, new[]
                    {
                        (error.Message ?? String.Empty).ToJsValue(engine),
                        (error.FileName ?? String.Empty).ToJsValue(engine),
                        error.Line.ToJsValue(engine),
                        error.Column.ToJsValue(engine),
                        error.Error.ToJsValue(engine),
                    });

                    if (handled.IsBoolean() && handled.AsBoolean())
                    {
                        ev.Cancel();
                    }
                }
                else
                {
                    var result = function.Call(sender, new[] { ev.ToJsValue(engine) });

                    if (result.IsBoolean() && !result.AsBoolean())
                    {
                        ev.Cancel();
                    }
                }
            }
            catch (JavaScriptException thrown)
            {
                engine.ReportError(thrown);
            }
            finally
            {
                engine.LeaveScript();
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
            //  A leading window (e.g., of a timer or an animation frame) is the receiver of the
            //  call, all other parameters are its arguments (e.g., the records and the observer
            //  of a MutationCallback, the node of a NodeFilter). Value types (e.g., the time of
            //  an animation frame) have to be boxed for the conversion.
            Expression ToJsValue(ParameterExpression m) =>
                Expression.Call(convert, Expression.Convert(m, typeof(Object)), engineExpr);
            var hasReceiver = parameters.Length > 0 && typeof(IWindow).IsAssignableFrom(parameters[0].Type);
            var callback = methodInfo.ReturnType == typeof(Task) || methodInfo.ReturnType == typeof(Task<Object>)
                ? Expression.Lambda<T>(Expression.Convert(
                    Expression.Call(engineExpr, nameof(EngineInstance.RunCallback), Type.EmptyTypes,
                        objExpr,
                        hasReceiver ? Expression.Convert(parameters[0], typeof(Object)) : Expression.Constant(null, typeof(Object)),
                        Expression.NewArrayInit(typeof(Object), parameters.Skip(hasReceiver ? 1 : 0).Select(m => (Expression)Expression.Convert(m, typeof(Object))))),
                    methodInfo.ReturnType), parameters)
                : CreateCallback<T>(methodInfo, objExpr, engineExpr, parameters, hasReceiver, ToJsValue);
            return Expression.Lambda<Func<Function, EngineInstance, T>>(callback, objExpr, engineExpr).Compile();
        }

        private static Expression<T> CreateCallback<T>(MethodInfo methodInfo, ParameterExpression objExpr, ParameterExpression engineExpr, ParameterExpression[] parameters, Boolean hasReceiver, Func<ParameterExpression, Expression> ToJsValue)
        {
            var call = Expression.Call(objExpr, "Call", new Type[0], new Expression[]
            {
                hasReceiver ? ToJsValue(parameters[0]) : Expression.Constant(JsValue.Undefined, typeof(JsValue)),
                Expression.NewArrayInit(typeof(JsValue), parameters.Skip(hasReceiver ? 1 : 0).Select(ToJsValue).ToArray())
            });

            //  Nobody waits for what a callback without a result does, e.g. a timer, so an error
            //  ends here. A callback with a result (a NodeFilter) throws to whoever called for it.
            var error = Expression.Parameter(typeof(JavaScriptException), "error");
            var body = methodInfo.ReturnType == typeof(void)
                ? Expression.TryCatch(
                    Expression.Block(typeof(void), call),
                    Expression.Catch(error, Expression.Call(engineExpr, nameof(EngineInstance.ReportError), Type.EmptyTypes, error)))
                : (Expression)Expression.Convert(
                    Expression.Call(typeof(JsValueExtensions), nameof(JsValueExtensions.As), Type.EmptyTypes, call, Expression.Constant(methodInfo.ReturnType, typeof(Type)), engineExpr),
                    methodInfo.ReturnType);
            var run = Expression.Block(
                body.Type,
                Expression.Call(engineExpr, nameof(EngineInstance.EnterScript), Type.EmptyTypes),
                Expression.TryFinally(body, Expression.Call(engineExpr, nameof(EngineInstance.LeaveScript), Type.EmptyTypes)));
            return Expression.Lambda<T>(run, parameters);
        }
    }
}
