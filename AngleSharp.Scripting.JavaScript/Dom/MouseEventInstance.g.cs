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
        public MouseEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static MouseEventInstance CreateMouseEventObject(Engine engine)
        {
            var obj = new MouseEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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