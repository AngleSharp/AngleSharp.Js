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
        public TouchInstance(Engine engine)
            : base(engine)
        {
        }

        public static TouchInstance CreateTouchObject(Engine engine)
        {
            var obj = new TouchInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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