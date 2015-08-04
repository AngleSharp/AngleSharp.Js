namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class EventInstance : ObjectInstance
    {
        public EventInstance(Engine engine)
            : base(engine)
        {
            FastAddProperty("NONE", (UInt32)(AngleSharp.Dom.Events.EventPhase.None), false, true, false);
            FastAddProperty("CAPTURING_PHASE", (UInt32)(AngleSharp.Dom.Events.EventPhase.Capturing), false, true, false);
            FastAddProperty("AT_TARGET", (UInt32)(AngleSharp.Dom.Events.EventPhase.AtTarget), false, true, false);
            FastAddProperty("BUBBLING_PHASE", (UInt32)(AngleSharp.Dom.Events.EventPhase.Bubbling), false, true, false);
        }

        public static EventInstance CreateEventObject(Engine engine)
        {
            var obj = new EventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Event"; }
        }

        public Event RefEvent
        {
            get;
            set;
        }
    }
}