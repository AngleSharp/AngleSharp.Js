namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLUListElementInstance : HTMLElementInstance
    {
        public HTMLUListElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLUListElementInstance CreateHTMLUListElementObject(Engine engine)
        {
            var obj = new HTMLUListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLUListElement"; }
        }

        public IHtmlUnorderedListElement RefHTMLUListElement
        {
            get;
            set;
        }
    }
}