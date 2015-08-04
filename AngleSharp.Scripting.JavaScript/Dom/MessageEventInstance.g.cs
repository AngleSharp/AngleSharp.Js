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
        public MessageEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static MessageEventInstance CreateMessageEventObject(Engine engine)
        {
            var obj = new MessageEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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