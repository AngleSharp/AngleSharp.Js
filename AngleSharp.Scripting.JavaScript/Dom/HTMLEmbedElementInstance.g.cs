namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLEmbedElementInstance : HTMLElementInstance
    {
        public HTMLEmbedElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLEmbedElementInstance CreateHTMLEmbedElementObject(Engine engine)
        {
            var obj = new HTMLEmbedElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLEmbedElement"; }
        }

        public IHtmlEmbedElement RefHTMLEmbedElement
        {
            get;
            set;
        }
    }
}