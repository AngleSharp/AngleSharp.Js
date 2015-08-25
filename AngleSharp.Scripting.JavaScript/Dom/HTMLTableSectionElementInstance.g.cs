namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableSectionElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableSectionElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableSectionElementInstance CreateHTMLTableSectionElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableSectionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableSectionElement"; }
        }

        public IHtmlTableSectionElement RefHTMLTableSectionElement
        {
            get;
            set;
        }
    }
}