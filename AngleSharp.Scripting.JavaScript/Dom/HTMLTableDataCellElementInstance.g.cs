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
        public HTMLTableDataCellElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableDataCellElementInstance CreateHTMLTableDataCellElementObject(Engine engine)
        {
            var obj = new HTMLTableDataCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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