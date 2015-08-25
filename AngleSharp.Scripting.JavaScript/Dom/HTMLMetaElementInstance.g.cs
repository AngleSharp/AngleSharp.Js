namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMetaElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMetaElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLMetaElementInstance CreateHTMLMetaElementObject(EngineInstance engine)
        {
            var obj = new HTMLMetaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMetaElement"; }
        }

        public IHtmlMetaElement RefHTMLMetaElement
        {
            get;
            set;
        }
    }
}