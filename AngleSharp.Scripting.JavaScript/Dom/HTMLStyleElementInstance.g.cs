namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLStyleElementInstance : HTMLElementInstance
    {
        public HTMLStyleElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLStyleElementInstance CreateHTMLStyleElementObject(Engine engine)
        {
            var obj = new HTMLStyleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLStyleElement"; }
        }

        public IHtmlStyleElement RefHTMLStyleElement
        {
            get;
            set;
        }
    }
}