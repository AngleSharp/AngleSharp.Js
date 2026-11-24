namespace AngleSharp.Js
{
    using AngleSharp.Scripting;
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
    /// to its delegate type instead, in code the generator writes.
    /// </remarks>
    sealed class JsCallback : DomCallback
    {
        private readonly Function _function;
        private readonly EngineInstance _engine;

        public JsCallback(Function function, EngineInstance engine)
        {
            _function = function;
            _engine = engine;
        }

        protected override Object CallCore(Object thisArgument, Object[] arguments, Type resultType)
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
                var result = _function.Call(receiver, values);
                return resultType != null ? result.As(resultType, _engine) : result.FromJsValue();
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
