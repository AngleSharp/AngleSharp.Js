namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLUListElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLUListElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLUListElementInstance CreateHTMLUListElementObject(EngineInstance engine)
        {
            var obj = new HTMLUListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLUListElement"; }
        }

        public IHtmlUnorderedListElement RefHTMLUListElement
        {
            get;
            set;
        }
    }
}