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

        private JsValue GetEventHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();
            var registration = node?.GetEventHandler(this);
            return registration?.Function ?? JsValue.Null;
        }

        private JsValue SetEventHandler(JsValue thisObject, JsValue[] arguments)
        {
            var node = thisObject.As<DomNodeInstance>();

            if (node != null)
            {
                var previous = node.RemoveEventHandler(this);

                if (previous != null)
                {
                    _removeHandler?.Invoke(node.Value, new Object[] { previous.Handler });
                }

                if (arguments[0] is Function function)
                {
                    DomEventHandler handler = (s, ev) =>
                    {
                        var sender = s.ToJsValue(_engine);
                        var args = ev.ToJsValue(_engine);
                        function.Call(sender, new[] { args });
                    };

                    node.SetEventHandler(this, new Registration(function, handler));
                    _addHandler?.Invoke(node.Value, new Object[] { handler });
                }
            }

            return arguments[0];
        }

        /// <summary>
        /// The handler currently assigned to a single node for a single event.
        /// </summary>
        public sealed class Registration
        {
            public Registration(Function function, DomEventHandler handler)
            {
                Function = function;
                Handler = handler;
            }

            /// <summary>
            /// The function that was assigned, as it has to be handed back on read.
            /// </summary>
            public Function Function { get; }

            /// <summary>
            /// The listener that was subscribed, as it has to be handed back on removal.
            /// </summary>
            public DomEventHandler Handler { get; }
        }
    }
}
