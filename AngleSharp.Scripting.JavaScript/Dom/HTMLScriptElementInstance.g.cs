namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLScriptElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLScriptElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLScriptElementInstance CreateHTMLScriptElementObject(EngineInstance engine)
        {
            var obj = new HTMLScriptElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLScriptElement"; }
        }

        public IHtmlScriptElement RefHTMLScriptElement
        {
            get;
            set;
        }
    }
}