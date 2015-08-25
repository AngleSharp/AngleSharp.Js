namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOListElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLOListElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLOListElementInstance CreateHTMLOListElementObject(EngineInstance engine)
        {
            var obj = new HTMLOListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOListElement"; }
        }

        public IHtmlOrderedListElement RefHTMLOListElement
        {
            get;
            set;
        }
    }
}