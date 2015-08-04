namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLElementInstance : ElementInstance
    {
        public HTMLElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLElementInstance CreateHTMLElementObject(Engine engine)
        {
            var obj = new HTMLElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLElement"; }
        }

        public IHtmlElement RefHTMLElement
        {
            get;
            set;
        }
    }
}