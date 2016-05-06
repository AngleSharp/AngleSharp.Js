namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime.Interop;
    using System.Reflection;

    sealed class DomEventInstance
    {
        DomEventHandler _handler;
        FunctionInstance _function;

        public DomEventInstance(DomNodeInstance node, EventInfo eventInfo = null)
        {
            Getter = new ClrFunctionInstance(node.Engine, (thisObject, arguments) => _function ?? JsValue.Null);
            Setter = new ClrFunctionInstance(node.Engine, (thisObject, arguments) =>
            {
                if (_handler != null)
                {
                    if (eventInfo != null)
                    {
                        eventInfo.RemoveEventHandler(node.Value, _handler);
                    }

                    _handler = null;
                    _function = null;
                }

                if (arguments[0].Is<FunctionInstance>())
                {
                    _function = arguments[0].As<FunctionInstance>();
                    _handler = (s, ev) => 
                    {
                        var sender = s.ToJsValue(node.Context);
                        var args = ev.ToJsValue(node.Context);
                        _function.Call(sender, new [] { args });
                    };

                    if (eventInfo != null)
                    {
                        eventInfo.AddEventHandler(node.Value, _handler);
                    }
                }

                return arguments[0];
            });
        }

        public ClrFunctionInstance Getter
        {
            get;
            private set;
        }

        public ClrFunctionInstance Setter
        {
            get;
            private set;
        }
    }
}
