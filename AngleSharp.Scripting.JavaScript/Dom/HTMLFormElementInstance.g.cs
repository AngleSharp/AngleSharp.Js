namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLFormElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLFormElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLFormElementInstance CreateHTMLFormElementObject(EngineInstance engine)
        {
            var obj = new HTMLFormElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLFormElement"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefHTMLFormElement[index]);
            if (propertyName != null)
                return _engine.GetDomNode(RefHTMLFormElement[propertyName]);

            return base.Get(propertyName);
        }


        public IHtmlFormElement RefHTMLFormElement
        {
            get;
            set;
        }
    }
}