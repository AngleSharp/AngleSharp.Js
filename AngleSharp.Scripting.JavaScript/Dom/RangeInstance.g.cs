namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class RangeInstance : ObjectInstance
    {
        public RangeInstance(Engine engine)
            : base(engine)
        {
            FastAddProperty("START_TO_START", (UInt32)(AngleSharp.Dom.RangeType.StartToStart), false, true, false);
            FastAddProperty("START_TO_END", (UInt32)(AngleSharp.Dom.RangeType.StartToEnd), false, true, false);
            FastAddProperty("END_TO_END", (UInt32)(AngleSharp.Dom.RangeType.EndToEnd), false, true, false);
            FastAddProperty("END_TO_START", (UInt32)(AngleSharp.Dom.RangeType.EndToStart), false, true, false);
        }

        public static RangeInstance CreateRangeObject(Engine engine)
        {
            var obj = new RangeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Range"; }
        }

        public IRange RefRange
        {
            get;
            set;
        }
    }
}