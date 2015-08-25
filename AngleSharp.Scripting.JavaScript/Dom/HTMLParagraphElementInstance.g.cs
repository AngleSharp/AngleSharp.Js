namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLParagraphElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLParagraphElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLParagraphElementInstance CreateHTMLParagraphElementObject(EngineInstance engine)
        {
            var obj = new HTMLParagraphElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLParagraphElement"; }
        }

        public IHtmlParagraphElement RefHTMLParagraphElement
        {
            get;
            set;
        }
    }
}