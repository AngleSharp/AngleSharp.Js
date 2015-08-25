namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLSpanElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLSpanElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLSpanElementInstance CreateHTMLSpanElementObject(EngineInstance engine)
        {
            var obj = new HTMLSpanElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLSpanElement"; }
        }

        public IHtmlSpanElement RefHTMLSpanElement
        {
            get;
            set;
        }
    }
}