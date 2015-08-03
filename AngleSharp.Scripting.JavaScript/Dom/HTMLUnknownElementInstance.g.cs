namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLUnknownElementInstance : HTMLElementInstance
    {
        public HTMLUnknownElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLUnknownElementInstance CreateHTMLUnknownElementObject(Engine engine)
        {
            var obj = new HTMLUnknownElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLUnknownElement"; }
        }

        public IHtmlUnknownElement RefHTMLUnknownElement
        {
            get;
            set;
        }
    }
}