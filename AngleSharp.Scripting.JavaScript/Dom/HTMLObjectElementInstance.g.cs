namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLObjectElementInstance : HTMLElementInstance
    {
        public HTMLObjectElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLObjectElementInstance CreateHTMLObjectElementObject(Engine engine)
        {
            var obj = new HTMLObjectElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLObjectElement"; }
        }

        public IHtmlObjectElement RefHTMLObjectElement
        {
            get;
            set;
        }
    }
}