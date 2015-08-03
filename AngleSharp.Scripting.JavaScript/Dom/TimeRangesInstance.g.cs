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
        public TimeRangesInstance(Engine engine)
            : base(engine)
        {
        }

        public static TimeRangesInstance CreateTimeRangesObject(Engine engine)
        {
            var obj = new TimeRangesInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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