namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableRowElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableRowElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableRowElementInstance CreateHTMLTableRowElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableRowElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableRowElement"; }
        }

        public IHtmlTableRowElement RefHTMLTableRowElement
        {
            get;
            set;
        }
    }
}