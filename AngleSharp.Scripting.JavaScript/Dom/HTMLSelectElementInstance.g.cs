namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLSelectElementInstance : HTMLElementInstance
    {
        public HTMLSelectElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLSelectElementInstance CreateHTMLSelectElementObject(Engine engine)
        {
            var obj = new HTMLSelectElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLSelectElement"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return Engine.Select(RefHTMLSelectElement[index]);
            return base.Get(propertyName);
        }

        
        public override void Put(String propertyName, JsValue value, Boolean throwOnError)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
            {
                RefHTMLSelectElement[index] = DomTypeConverter.ToOptionElement(value);
                return;
            }

            base.Put(propertyName, value, throwOnError);
        }


        public IHtmlSelectElement RefHTMLSelectElement
        {
            get;
            set;
        }
    }
}