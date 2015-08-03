namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTemplateElementInstance : HTMLElementInstance
    {
        public HTMLTemplateElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTemplateElementInstance CreateHTMLTemplateElementObject(Engine engine)
        {
            var obj = new HTMLTemplateElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTemplateElement"; }
        }

        public IHtmlTemplateElement RefHTMLTemplateElement
        {
            get;
            set;
        }
    }
}