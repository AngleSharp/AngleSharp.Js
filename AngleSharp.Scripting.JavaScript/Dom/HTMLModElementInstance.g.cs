namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLModElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLModElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLModElementInstance CreateHTMLModElementObject(EngineInstance engine)
        {
            var obj = new HTMLModElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLModElement"; }
        }

        public IHtmlModElement RefHTMLModElement
        {
            get;
            set;
        }
    }
}