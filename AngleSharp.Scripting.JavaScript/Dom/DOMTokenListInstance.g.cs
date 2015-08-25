namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DOMTokenListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public DOMTokenListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static DOMTokenListInstance CreateDOMTokenListObject(EngineInstance engine)
        {
            var obj = new DOMTokenListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DOMTokenList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefDOMTokenList[index]);
            return base.Get(propertyName);
        }


        public ITokenList RefDOMTokenList
        {
            get;
            set;
        }
    }
}