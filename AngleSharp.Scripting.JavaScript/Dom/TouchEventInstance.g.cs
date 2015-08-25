namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TouchEventInstance : UIEventInstance
    {
        readonly EngineInstance _engine;

        public TouchEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static TouchEventInstance CreateTouchEventObject(EngineInstance engine)
        {
            var obj = new TouchEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TouchEvent"; }
        }

        public TouchEvent RefTouchEvent
        {
            get;
            set;
        }
    }
}