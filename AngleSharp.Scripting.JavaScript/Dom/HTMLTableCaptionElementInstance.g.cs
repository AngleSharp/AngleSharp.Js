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
        readonly EngineInstance _engine;

        public HTMLTableCaptionElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableCaptionElementInstance CreateHTMLTableCaptionElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableCaptionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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