namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLEmbedElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLEmbedElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLEmbedElementInstance CreateHTMLEmbedElementObject(EngineInstance engine)
        {
            var obj = new HTMLEmbedElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLEmbedElement"; }
        }

        public IHtmlEmbedElement RefHTMLEmbedElement
        {
            get;
            set;
        }
    }
}