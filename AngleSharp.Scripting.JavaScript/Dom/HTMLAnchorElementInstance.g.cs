namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAnchorElementInstance : HTMLElementInstance
    {
        public HTMLAnchorElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLAnchorElementInstance CreateHTMLAnchorElementObject(Engine engine)
        {
            var obj = new HTMLAnchorElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAnchorElement"; }
        }

        public IHtmlAnchorElement RefHTMLAnchorElement
        {
            get;
            set;
        }
    }
}