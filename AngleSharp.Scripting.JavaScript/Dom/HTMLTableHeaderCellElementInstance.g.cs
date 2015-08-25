namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableHeaderCellElementInstance : HTMLTableCellElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableHeaderCellElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableHeaderCellElementInstance CreateHTMLTableHeaderCellElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableHeaderCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableHeaderCellElement"; }
        }

        public IHtmlTableHeaderCellElement RefHTMLTableHeaderCellElement
        {
            get;
            set;
        }
    }
}