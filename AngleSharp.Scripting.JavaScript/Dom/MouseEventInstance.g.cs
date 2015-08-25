namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MouseEventInstance : UIEventInstance
    {
        readonly EngineInstance _engine;

        public MouseEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static MouseEventInstance CreateMouseEventObject(EngineInstance engine)
        {
            var obj = new MouseEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MouseEvent"; }
        }

        public MouseEvent RefMouseEvent
        {
            get;
            set;
        }
    }
}