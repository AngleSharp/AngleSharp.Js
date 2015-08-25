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
        readonly EngineInstance _engine;

        public TrackEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TrackEventInstance CreateTrackEventObject(EngineInstance engine)
        {
            var obj = new TrackEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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