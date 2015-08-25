namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class FocusEventInstance : UIEventInstance
    {
        readonly EngineInstance _engine;

        public FocusEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static FocusEventInstance CreateFocusEventObject(EngineInstance engine)
        {
            var obj = new FocusEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "FocusEvent"; }
        }

        public FocusEvent RefFocusEvent
        {
            get;
            set;
        }
    }
}