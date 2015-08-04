namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLQuoteElementInstance : HTMLElementInstance
    {
        public HTMLQuoteElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLQuoteElementInstance CreateHTMLQuoteElementObject(Engine engine)
        {
            var obj = new HTMLQuoteElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLQuoteElement"; }
        }

        public IHtmlQuoteElement RefHTMLQuoteElement
        {
            get;
            set;
        }
    }
}