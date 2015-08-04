namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLinkElementInstance : HTMLElementInstance
    {
        public HTMLLinkElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLLinkElementInstance CreateHTMLLinkElementObject(Engine engine)
        {
            var obj = new HTMLLinkElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLinkElement"; }
        }

        public IHtmlLinkElement RefHTMLLinkElement
        {
            get;
            set;
        }
    }
}