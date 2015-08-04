namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TrackEventInstance : EventInstance
    {
        public TrackEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static TrackEventInstance CreateTrackEventObject(Engine engine)
        {
            var obj = new TrackEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TrackEvent"; }
        }

        public TrackEvent RefTrackEvent
        {
            get;
            set;
        }
    }
}