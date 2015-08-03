namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLScriptElementInstance : HTMLElementInstance
    {
        public HTMLScriptElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLScriptElementInstance CreateHTMLScriptElementObject(Engine engine)
        {
            var obj = new HTMLScriptElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLScriptElement"; }
        }

        public IHtmlScriptElement RefHTMLScriptElement
        {
            get;
            set;
        }
    }
}