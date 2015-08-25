namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableCellElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableCellElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableCellElementInstance CreateHTMLTableCellElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableCellElement"; }
        }

        public IHtmlTableCellElement RefHTMLTableCellElement
        {
            get;
            set;
        }
    }
}