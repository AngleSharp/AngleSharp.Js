namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLElementInstance : ElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLElementInstance CreateHTMLElementObject(EngineInstance engine)
        {
            var obj = new HTMLElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLElement"; }
        }

        public IHtmlElement RefHTMLElement
        {
            get;
            set;
        }
    }
}