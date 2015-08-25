namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTemplateElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTemplateElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTemplateElementInstance CreateHTMLTemplateElementObject(EngineInstance engine)
        {
            var obj = new HTMLTemplateElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTemplateElement"; }
        }

        public IHtmlTemplateElement RefHTMLTemplateElement
        {
            get;
            set;
        }
    }
}