namespace AngleSharp.Js
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using System;

    /// <summary>
    /// A function script passed to a registered operation.
    /// </summary>
    /// <remarks>
    /// The reflection path turns a callback into whatever delegate type the parameter has by
    /// compiling an expression tree, which native AoT cannot do. A registration adapts this object
    /// to its delegate type instead, in code the generator writes:
    /// <c>args.GetCallback(0) is JsCallback cb ? (a, b) => cb.Invoke(a, b) : null</c>.
    /// </remarks>
    public sealed class JsCallback
    {
        private readonly Function _function;
        private readonly EngineInstance _engine;

        internal JsCallback(Function function, EngineInstance engine)
        {
            _function = function;
            _engine = engine;
        }

        /// <summary>
        /// Calls the function with undefined as this.
        /// </summary>
        /// <param name="arguments">The arguments to pass.</param>
        /// <returns>The result of the function, or null if it threw.</returns>
        public Object Invoke(params Object[] arguments) => Call(null, arguments);

        /// <summary>
        /// Calls the function. What it throws is reported to the window like an error of a
        /// listener, as nothing on the CLR side could catch it meaningfully.
        /// </summary>
        /// <param name="thisArgument">The receiver of the call.</param>
        /// <param name="arguments">The arguments to pass.</param>
        /// <returns>The result of the function, or null if it threw.</returns>
        public Object Call(Object thisArgument, params Object[] arguments)
        {
            arguments = arguments ?? Array.Empty<Object>();
            var values = new JsValue[arguments.Length];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = arguments[i].ToJsValue(_engine);
            }

            _engine.EnterScript();

            try
            {
                var receiver = thisArgument != null ? thisArgument.ToJsValue(_engine) : JsValue.Undefined;
                return _function.Call(receiver, values).FromJsValue();
            }
            catch (JavaScriptException error)
            {
                _engine.ReportError(error);
                return null;
            }
            finally
            {
                _engine.LeaveScript();
            }
        }
    }
}
