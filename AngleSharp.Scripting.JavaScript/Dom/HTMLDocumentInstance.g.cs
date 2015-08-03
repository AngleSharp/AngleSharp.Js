namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDocumentInstance : DocumentInstance
    {
        public HTMLDocumentInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDocumentInstance CreateHTMLDocumentObject(Engine engine)
        {
            var obj = new HTMLDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDocument"; }
        }

        public IHtmlDocument RefHTMLDocument
        {
            get;
            set;
        }
    }
}