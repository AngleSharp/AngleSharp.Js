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
        public HTMLFormElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLFormElementInstance CreateHTMLFormElementObject(Engine engine)
        {
            var obj = new HTMLFormElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefHTMLFormElement[index]);
            if (propertyName != null)
                return Engine.Select(RefHTMLFormElement[propertyName]);

            return base.Get(propertyName);
        }


        public IHtmlFormElement RefHTMLFormElement
        {
            get;
            set;
        }
    }
}