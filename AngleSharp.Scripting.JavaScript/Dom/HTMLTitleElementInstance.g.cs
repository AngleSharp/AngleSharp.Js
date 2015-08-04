namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTitleElementInstance : HTMLElementInstance
    {
        public HTMLTitleElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTitleElementInstance CreateHTMLTitleElementObject(Engine engine)
        {
            var obj = new HTMLTitleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTitleElement"; }
        }

        public IHtmlTitleElement RefHTMLTitleElement
        {
            get;
            set;
        }
    }
}