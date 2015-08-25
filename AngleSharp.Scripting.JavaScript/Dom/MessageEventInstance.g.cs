namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MessageEventInstance : EventInstance
    {
        readonly EngineInstance _engine;

        public MessageEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static MessageEventInstance CreateMessageEventObject(EngineInstance engine)
        {
            var obj = new MessageEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MessageEvent"; }
        }

        public MessageEvent RefMessageEvent
        {
            get;
            set;
        }
    }
}