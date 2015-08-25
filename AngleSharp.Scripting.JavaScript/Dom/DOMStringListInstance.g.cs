namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMStringListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public DOMStringListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static DOMStringListInstance CreateDOMStringListObject(EngineInstance engine)
        {
            var obj = new DOMStringListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMStringList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefDOMStringList[index]);
            return base.Get(propertyName);
        }


        public IStringList RefDOMStringList
        {
            get;
            set;
        }
    }
}