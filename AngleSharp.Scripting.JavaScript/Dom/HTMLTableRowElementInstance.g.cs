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
        public HTMLTableRowElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableRowElementInstance CreateHTMLTableRowElementObject(Engine engine)
        {
            var obj = new HTMLTableRowElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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