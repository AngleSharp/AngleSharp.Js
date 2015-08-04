namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOptionElementInstance : HTMLElementInstance
    {
        public HTMLOptionElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLOptionElementInstance CreateHTMLOptionElementObject(Engine engine)
        {
            var obj = new HTMLOptionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOptionElement"; }
        }

        public IHtmlOptionElement RefHTMLOptionElement
        {
            get;
            set;
        }
    }
}