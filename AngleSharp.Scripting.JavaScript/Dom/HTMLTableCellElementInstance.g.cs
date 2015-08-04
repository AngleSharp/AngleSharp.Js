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
        public HTMLTableCellElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableCellElementInstance CreateHTMLTableCellElementObject(Engine engine)
        {
            var obj = new HTMLTableCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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