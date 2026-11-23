namespace AngleSharp.Js.Proxies
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A function of a script behind a variadic delegate, i.e. one taking all of its arguments
    /// as a list (e.g. the host functions a WebAssembly module imports), which the library calls
    /// with any number of them. An argument is converted as for any member of the DOM; an
    /// undefined (the result too) is <see cref="Missing.Value"/>.
    /// </summary>
    sealed class ScriptCallback
    {
        private static readonly MethodInfo InvokeMethod = typeof(ScriptCallback).GetTypeInfo().GetDeclaredMethod(nameof(Invoke));
        private static readonly MethodInfo InvokeVoidMethod = typeof(ScriptCallback).GetTypeInfo().GetDeclaredMethod(nameof(InvokeVoid));

        private ScriptCallback(Function function, EngineInstance engine)
        {
            Function = function;
            Engine = engine;
        }

        public Function Function { get; }

        public EngineInstance Engine { get; }

        /// <summary>
        /// Gets if the delegate type takes its arguments as a list: its one parameter is an
        /// Object[] (or a read-only list of objects), and it gives an object or nothing.
        /// </summary>
        public static Boolean IsVariadic(Type type)
        {
            var invoke = type.GetTypeInfo().GetDeclaredMethod("Invoke");
            var parameters = invoke?.GetParameters();

            return parameters != null && parameters.Length == 1 &&
                (parameters[0].ParameterType == typeof(Object[]) || parameters[0].ParameterType == typeof(IReadOnlyList<Object>)) &&
                (invoke.ReturnType == typeof(Object) || invoke.ReturnType == typeof(void));
        }

        public static Delegate Create(Type type, Function function, EngineInstance engine)
        {
            var invoke = type.GetTypeInfo().GetDeclaredMethod("Invoke");
            var target = new ScriptCallback(function, engine);
            return InvokeMethod.ReturnType == invoke.ReturnType
                ? InvokeMethod.CreateDelegate(type, target)
                : InvokeVoidMethod.CreateDelegate(type, target);
        }

        private Object Invoke(IReadOnlyList<Object> arguments)
        {
            var values = new JsValue[arguments?.Count ?? 0];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = arguments[i] == Missing.Value ? JsValue.Undefined : arguments[i].ToJsValue(Engine);
            }

            Engine.EnterScript();

            try
            {
                return ToClr(Engine.Jint.Call(Function, JsValue.Undefined, values));
            }
            finally
            {
                Engine.LeaveScript();
            }
        }

        private void InvokeVoid(IReadOnlyList<Object> arguments) => Invoke(arguments);

        /// <summary>
        /// Converts what a script gave to what a variadic delegate takes or gives.
        /// </summary>
        public static Object ToClr(JsValue value) => value.IsUndefined() ? Missing.Value : value.FromJsValue();

        /// <summary>
        /// Calls a variadic delegate with the given arguments; compiled once per delegate type.
        /// </summary>
        public static Object InvokeVariadic(Delegate callback, Object[] arguments) =>
            Invokers.GetOrAdd(callback.GetType(), CreateInvoker)(callback, arguments);

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, Func<Delegate, Object[], Object>> Invokers =
            new System.Collections.Concurrent.ConcurrentDictionary<Type, Func<Delegate, Object[], Object>>();

        private static Func<Delegate, Object[], Object> CreateInvoker(Type type)
        {
            var invoke = type.GetTypeInfo().GetDeclaredMethod("Invoke");
            var callback = Expression.Parameter(typeof(Delegate), "callback");
            var arguments = Expression.Parameter(typeof(Object[]), "arguments");
            var call = Expression.Call(Expression.Convert(callback, type), invoke, Expression.Convert(arguments, invoke.GetParameters()[0].ParameterType));
            var body = invoke.ReturnType == typeof(void)
                ? (Expression)Expression.Block(call, Expression.Constant(null, typeof(Object)))
                : Expression.Convert(call, typeof(Object));
            return Expression.Lambda<Func<Delegate, Object[], Object>>(body, callback, arguments).Compile();
        }
    }
}
