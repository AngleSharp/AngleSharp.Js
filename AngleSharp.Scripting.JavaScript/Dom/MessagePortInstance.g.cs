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
        readonly EngineInstance _engine;

        public MessagePortInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static MessagePortInstance CreateMessagePortObject(EngineInstance engine)
        {
            var obj = new MessagePortInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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