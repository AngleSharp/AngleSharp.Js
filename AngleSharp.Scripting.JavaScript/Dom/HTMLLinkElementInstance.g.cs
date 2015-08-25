namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLinkElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLLinkElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLLinkElementInstance CreateHTMLLinkElementObject(EngineInstance engine)
        {
            var obj = new HTMLLinkElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLinkElement"; }
        }

        public IHtmlLinkElement RefHTMLLinkElement
        {
            get;
            set;
        }
    }
}