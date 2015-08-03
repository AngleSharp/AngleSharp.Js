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
        public TouchEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static TouchEventInstance CreateTouchEventObject(Engine engine)
        {
            var obj = new TouchEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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