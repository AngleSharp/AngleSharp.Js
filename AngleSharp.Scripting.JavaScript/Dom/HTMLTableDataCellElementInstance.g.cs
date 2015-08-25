namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableDataCellElementInstance : HTMLTableCellElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableDataCellElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableDataCellElementInstance CreateHTMLTableDataCellElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableDataCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableDataCellElement"; }
        }

        public IHtmlTableDataCellElement RefHTMLTableDataCellElement
        {
            get;
            set;
        }
    }
}