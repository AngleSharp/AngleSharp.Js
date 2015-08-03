namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOListElementInstance : HTMLElementInstance
    {
        public HTMLOListElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLOListElementInstance CreateHTMLOListElementObject(Engine engine)
        {
            var obj = new HTMLOListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOListElement"; }
        }

        public IHtmlOrderedListElement RefHTMLOListElement
        {
            get;
            set;
        }
    }
}