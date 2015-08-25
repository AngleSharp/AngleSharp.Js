namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TouchInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public TouchInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static TouchInstance CreateTouchObject(EngineInstance engine)
        {
            var obj = new TouchInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Touch"; }
        }

        public ITouchPoint RefTouch
        {
            get;
            set;
        }
    }
}