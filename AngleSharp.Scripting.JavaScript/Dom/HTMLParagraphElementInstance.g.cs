namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLParagraphElementInstance : HTMLElementInstance
    {
        public HTMLParagraphElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLParagraphElementInstance CreateHTMLParagraphElementObject(Engine engine)
        {
            var obj = new HTMLParagraphElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLParagraphElement"; }
        }

        public IHtmlParagraphElement RefHTMLParagraphElement
        {
            get;
            set;
        }
    }
}