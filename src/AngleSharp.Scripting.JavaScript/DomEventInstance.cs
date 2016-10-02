namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime.Interop;
    using System.Reflection;

    sealed class DomEventInstance
    {
        private readonly EngineInstance _engine;
        private readonly EventInfo _eventInfo;
        private DomEventHandler _handler;
        private FunctionInstance _function;

        public DomEventInstance(EngineInstance engine, EventInfo eventInfo = null)
        {
            _engine = engine;
            _eventInfo = eventInfo;
            Getter = new ClrFunctionInstance(engine.Jint, GetEventHandler);
            Setter = new ClrFunctionInstance(engine.Jint, SetEventHandler);
        }

        public ClrFunctionInstance Getter { get; }

        public ClrFunctionInstance Setter { get; }

        private JsValue GetEventHandler(JsValue thisObject, JsValue[] arguments)
        {
            return _function ?? JsValue.Null;
        }

        private JsValue SetEventHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();

            if (node != null)
            {
                if (_handler != null)
                {
                    _eventInfo?.RemoveEventHandler(node.Value, _handler);
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

                    _eventInfo?.AddEventHandler(node.Value, _handler);
                }
            }

            return arguments[0];
        }
    }
}
