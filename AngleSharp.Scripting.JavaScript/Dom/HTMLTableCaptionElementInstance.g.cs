namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableCaptionElementInstance : HTMLElementInstance
    {
        public HTMLTableCaptionElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableCaptionElementInstance CreateHTMLTableCaptionElementObject(Engine engine)
        {
            var obj = new HTMLTableCaptionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableCaptionElement"; }
        }

        public IHtmlTableCaptionElement RefHTMLTableCaptionElement
        {
            get;
            set;
        }
    }
}