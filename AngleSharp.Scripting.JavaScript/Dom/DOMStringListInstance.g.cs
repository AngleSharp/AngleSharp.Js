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
        public DOMStringListInstance(Engine engine)
            : base(engine)
        {
        }

        public static DOMStringListInstance CreateDOMStringListObject(Engine engine)
        {
            var obj = new DOMStringListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefDOMStringList[index]);
            return base.Get(propertyName);
        }


        public IStringList RefDOMStringList
        {
            get;
            set;
        }
    }
}