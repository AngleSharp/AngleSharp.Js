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
        public HTMLTableHeaderCellElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableHeaderCellElementInstance CreateHTMLTableHeaderCellElementObject(Engine engine)
        {
            var obj = new HTMLTableHeaderCellElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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