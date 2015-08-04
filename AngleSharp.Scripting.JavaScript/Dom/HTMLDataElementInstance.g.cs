namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDataElementInstance : HTMLElementInstance
    {
        public HTMLDataElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDataElementInstance CreateHTMLDataElementObject(Engine engine)
        {
            var obj = new HTMLDataElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDataElement"; }
        }

        public IHtmlDataElement RefHTMLDataElement
        {
            get;
            set;
        }
    }
}