namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDataListElementInstance : HTMLElementInstance
    {
        public HTMLDataListElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDataListElementInstance CreateHTMLDataListElementObject(Engine engine)
        {
            var obj = new HTMLDataListElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDataListElement"; }
        }

        public IHtmlDataListElement RefHTMLDataListElement
        {
            get;
            set;
        }
    }
}