namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class TouchListInstance : ObjectInstance
    {
        public TouchListInstance(Engine engine)
            : base(engine)
        {
        }

        public static TouchListInstance CreateTouchListObject(Engine engine)
        {
            var obj = new TouchListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "TouchList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return Engine.Select(RefTouchList[index]);
            return base.Get(propertyName);
        }


        public ITouchList RefTouchList
        {
            get;
            set;
        }
    }
}