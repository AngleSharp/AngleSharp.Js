namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTitleElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTitleElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTitleElementInstance CreateHTMLTitleElementObject(EngineInstance engine)
        {
            var obj = new HTMLTitleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTitleElement"; }
        }

        public IHtmlTitleElement RefHTMLTitleElement
        {
            get;
            set;
        }
    }
}