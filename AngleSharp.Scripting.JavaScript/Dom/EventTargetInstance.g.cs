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
        readonly EngineInstance _engine;

        public EventTargetInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static EventTargetInstance CreateEventTargetObject(EngineInstance engine)
        {
            var obj = new EventTargetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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