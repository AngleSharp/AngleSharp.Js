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
        readonly EngineInstance _engine;

        public TouchListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static TouchListInstance CreateTouchListObject(EngineInstance engine)
        {
            var obj = new TouchListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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
                return _engine.GetDomNode(RefTouchList[index]);
            return base.Get(propertyName);
        }


        public ITouchList RefTouchList
        {
            get;
            set;
        }
    }
}