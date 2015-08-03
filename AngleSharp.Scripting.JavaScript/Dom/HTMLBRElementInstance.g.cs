namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLBRElementInstance : HTMLElementInstance
    {
        public HTMLBRElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLBRElementInstance CreateHTMLBRElementObject(Engine engine)
        {
            var obj = new HTMLBRElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLBRElement"; }
        }

        public IHtmlBreakRowElement RefHTMLBRElement
        {
            get;
            set;
        }
    }
}