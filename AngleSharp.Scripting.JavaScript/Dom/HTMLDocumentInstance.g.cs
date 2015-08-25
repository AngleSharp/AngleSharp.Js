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
        readonly EngineInstance _engine;

        public HTMLDocumentInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDocumentInstance CreateHTMLDocumentObject(EngineInstance engine)
        {
            var obj = new HTMLDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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