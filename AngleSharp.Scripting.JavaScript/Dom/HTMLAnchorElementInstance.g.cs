namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAnchorElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLAnchorElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLAnchorElementInstance CreateHTMLAnchorElementObject(EngineInstance engine)
        {
            var obj = new HTMLAnchorElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAnchorElement"; }
        }

        public IHtmlAnchorElement RefHTMLAnchorElement
        {
            get;
            set;
        }
    }
}