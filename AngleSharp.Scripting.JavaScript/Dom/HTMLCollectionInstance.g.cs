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
        public HTMLCollectionInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLCollectionInstance CreateHTMLCollectionObject(Engine engine)
        {
            var obj = new HTMLCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefHTMLCollection[index]);
            if (propertyName != null)
                return Engine.Select(RefHTMLCollection[propertyName]);

            return base.Get(propertyName);
        }


        public IHtmlCollection<IElement> RefHTMLCollection
        {
            get;
            set;
        }
    }
}