namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TimeRangesInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public TimeRangesInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static TimeRangesInstance CreateTimeRangesObject(EngineInstance engine)
        {
            var obj = new TimeRangesInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TimeRanges"; }
        }

        public ITimeRanges RefTimeRanges
        {
            get;
            set;
        }
    }
}