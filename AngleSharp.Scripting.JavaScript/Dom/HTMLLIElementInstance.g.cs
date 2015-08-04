namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLIElementInstance : HTMLElementInstance
    {
        public HTMLLIElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLLIElementInstance CreateHTMLLIElementObject(Engine engine)
        {
            var obj = new HTMLLIElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLIElement"; }
        }

        public IHtmlListItemElement RefHTMLLIElement
        {
            get;
            set;
        }
    }
}