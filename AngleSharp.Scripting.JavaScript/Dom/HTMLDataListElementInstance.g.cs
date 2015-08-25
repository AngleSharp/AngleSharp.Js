namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDataListElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLDataListElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDataListElementInstance CreateHTMLDataListElementObject(EngineInstance engine)
        {
            var obj = new HTMLDataListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDataListElement"; }
        }

        public IHtmlDataListElement RefHTMLDataListElement
        {
            get;
            set;
        }
    }
}