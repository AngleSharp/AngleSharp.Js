namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMenuElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMenuElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLMenuElementInstance CreateHTMLMenuElementObject(EngineInstance engine)
        {
            var obj = new HTMLMenuElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMenuElement"; }
        }

        public IHtmlMenuElement RefHTMLMenuElement
        {
            get;
            set;
        }
    }
}