namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLCollectionInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public HTMLCollectionInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static HTMLCollectionInstance CreateHTMLCollectionObject(EngineInstance engine)
        {
            var obj = new HTMLCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLCollection"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefHTMLCollection[index]);
            if (propertyName != null)
                return _engine.GetDomNode(RefHTMLCollection[propertyName]);

            return base.Get(propertyName);
        }


        public IHtmlCollection<IElement> RefHTMLCollection
        {
            get;
            set;
        }
    }
}