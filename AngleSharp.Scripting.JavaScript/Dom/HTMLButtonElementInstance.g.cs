namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLButtonElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLButtonElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLButtonElementInstance CreateHTMLButtonElementObject(EngineInstance engine)
        {
            var obj = new HTMLButtonElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLButtonElement"; }
        }

        public IHtmlButtonElement RefHTMLButtonElement
        {
            get;
            set;
        }
    }
}