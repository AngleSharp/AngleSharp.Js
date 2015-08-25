namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLInputElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLInputElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLInputElementInstance CreateHTMLInputElementObject(EngineInstance engine)
        {
            var obj = new HTMLInputElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLInputElement"; }
        }

        public IHtmlInputElement RefHTMLInputElement
        {
            get;
            set;
        }
    }
}