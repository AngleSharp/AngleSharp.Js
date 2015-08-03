namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTextAreaElementInstance : HTMLElementInstance
    {
        public HTMLTextAreaElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTextAreaElementInstance CreateHTMLTextAreaElementObject(Engine engine)
        {
            var obj = new HTMLTextAreaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTextAreaElement"; }
        }

        public IHtmlTextAreaElement RefHTMLTextAreaElement
        {
            get;
            set;
        }
    }
}