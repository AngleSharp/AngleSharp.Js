namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLabelElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLLabelElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLLabelElementInstance CreateHTMLLabelElementObject(EngineInstance engine)
        {
            var obj = new HTMLLabelElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLabelElement"; }
        }

        public IHtmlLabelElement RefHTMLLabelElement
        {
            get;
            set;
        }
    }
}