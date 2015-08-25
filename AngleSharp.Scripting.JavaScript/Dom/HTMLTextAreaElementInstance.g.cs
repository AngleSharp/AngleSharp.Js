namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTextAreaElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTextAreaElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTextAreaElementInstance CreateHTMLTextAreaElementObject(EngineInstance engine)
        {
            var obj = new HTMLTextAreaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTextAreaElement"; }
        }

        public IHtmlTextAreaElement RefHTMLTextAreaElement
        {
            get;
            set;
        }
    }
}