namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOptionElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLOptionElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLOptionElementInstance CreateHTMLOptionElementObject(EngineInstance engine)
        {
            var obj = new HTMLOptionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOptionElement"; }
        }

        public IHtmlOptionElement RefHTMLOptionElement
        {
            get;
            set;
        }
    }
}