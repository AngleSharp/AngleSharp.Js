namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class EventTargetInstance : ObjectInstance
    {
        public EventTargetInstance(Engine engine)
            : base(engine)
        {
        }

        public static EventTargetInstance CreateEventTargetObject(Engine engine)
        {
            var obj = new EventTargetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "EventTarget"; }
        }

        public IEventTarget RefEventTarget
        {
            get;
            set;
        }
    }
}