namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLStyleElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLStyleElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLStyleElementInstance CreateHTMLStyleElementObject(EngineInstance engine)
        {
            var obj = new HTMLStyleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLStyleElement"; }
        }

        public IHtmlStyleElement RefHTMLStyleElement
        {
            get;
            set;
        }
    }
}