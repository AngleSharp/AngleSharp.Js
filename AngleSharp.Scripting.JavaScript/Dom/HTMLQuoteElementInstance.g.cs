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
        readonly EngineInstance _engine;

        public HTMLQuoteElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLQuoteElementInstance CreateHTMLQuoteElementObject(EngineInstance engine)
        {
            var obj = new HTMLQuoteElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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