namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MessagePortInstance : EventTargetInstance
    {
        public MessagePortInstance(Engine engine)
            : base(engine)
        {
        }

        public static MessagePortInstance CreateMessagePortObject(Engine engine)
        {
            var obj = new MessagePortInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MessagePort"; }
        }

        public IMessagePort RefMessagePort
        {
            get;
            set;
        }
    }
}