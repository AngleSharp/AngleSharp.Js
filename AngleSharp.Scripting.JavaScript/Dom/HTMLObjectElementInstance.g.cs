namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLObjectElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLObjectElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLObjectElementInstance CreateHTMLObjectElementObject(EngineInstance engine)
        {
            var obj = new HTMLObjectElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLObjectElement"; }
        }

        public IHtmlObjectElement RefHTMLObjectElement
        {
            get;
            set;
        }
    }
}