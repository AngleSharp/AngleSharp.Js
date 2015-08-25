namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOptionsCollectionInstance : HTMLCollectionInstance
    {
        readonly EngineInstance _engine;

        public HTMLOptionsCollectionInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLOptionsCollectionInstance CreateHTMLOptionsCollectionObject(EngineInstance engine)
        {
            var obj = new HTMLOptionsCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOptionsCollection"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefHTMLOptionsCollection.GetOptionAt(index));
            return base.Get(propertyName);
        }

        
        public override void Put(String propertyName, JsValue value, Boolean throwOnError)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
            {
                RefHTMLOptionsCollection.SetOptionAt(index, DomTypeConverter.ToOptionElement(value));
                return;
            }

            base.Put(propertyName, value, throwOnError);
        }


        public IHtmlOptionsCollection RefHTMLOptionsCollection
        {
            get;
            set;
        }
    }
}