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
        public HTMLTableSectionElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableSectionElementInstance CreateHTMLTableSectionElementObject(Engine engine)
        {
            var obj = new HTMLTableSectionElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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