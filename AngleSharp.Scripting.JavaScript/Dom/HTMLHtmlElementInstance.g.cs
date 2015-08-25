namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHtmlElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLHtmlElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLHtmlElementInstance CreateHTMLHtmlElementObject(EngineInstance engine)
        {
            var obj = new HTMLHtmlElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHtmlElement"; }
        }

        public IHtmlHtmlElement RefHTMLHtmlElement
        {
            get;
            set;
        }
    }
}