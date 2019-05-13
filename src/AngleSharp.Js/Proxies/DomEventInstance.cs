namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime.Interop;
    using System;
    using System.Reflection;

    sealed class DomEventInstance
    {
        private readonly EngineInstance _engine;
        private readonly MethodInfo _addHandler;
        private readonly MethodInfo _removeHandler;
        private DomEventHandler _handler;
        private FunctionInstance _function;

        public DomEventInstance(EngineInstance engine, MethodInfo addHandler, MethodInfo removeHandler)
        {
            _engine = engine;
            _addHandler = addHandler;
            _removeHandler = removeHandler;
            Getter = new ClrFunctionInstance(engine.Jint, GetEventHandler);
            Setter = new ClrFunctionInstance(engine.Jint, SetEventHandler);
        }

        public ClrFunctionInstance Getter { get; }

        public ClrFunctionInstance Setter { get; }

        private JsValue GetEventHandler(JsValue thisObject, JsValue[] arguments) =>
            _function ?? JsValue.Null;

        private JsValue SetEventHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();

            if (node != null)
            {
                if (_handler != null)
                {
                    _removeHandler?.Invoke(node.Value, new Object[] { _handler });
                    _handler = null;
                    _function = null;
                }

                if (arguments[0].Is<FunctionInstance>())
                {
                    _function = arguments[0].As<FunctionInstance>();
                    _handler = (s, ev) =>
                    {
                        var sender = s.ToJsValue(_engine);
                        var args = ev.ToJsValue(_engine);
                        _function.Call(sender, new[] { args });
                    };

                    _addHandler?.Invoke(node.Value, new Object[] { _handler });
                }
            }

            return arguments[0];
        }
    }
}
