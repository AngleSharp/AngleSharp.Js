namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLSpanElementInstance : HTMLElementInstance
    {
        public HTMLSpanElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLSpanElementInstance CreateHTMLSpanElementObject(Engine engine)
        {
            var obj = new HTMLSpanElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLSpanElement"; }
        }

        public IHtmlSpanElement RefHTMLSpanElement
        {
            get;
            set;
        }
    }
}