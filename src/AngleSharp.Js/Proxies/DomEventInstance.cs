namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint;
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
        private Function _function;

        public DomEventInstance(EngineInstance engine, MethodInfo addHandler, MethodInfo removeHandler)
        {
            _engine = engine;
            _addHandler = addHandler;
            _removeHandler = removeHandler;
            Getter = new ClrFunction(engine.Jint, "get", GetEventHandler);
            Setter = new ClrFunction(engine.Jint, "set", SetEventHandler);
        }

        public ClrFunction Getter { get; }

        public ClrFunction Setter { get; }

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

                if (arguments[0] is Function)
                {
                    _function = arguments[0].As<Function>();
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
