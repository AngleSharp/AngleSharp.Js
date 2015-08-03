namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMetaElementInstance : HTMLElementInstance
    {
        public HTMLMetaElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLMetaElementInstance CreateHTMLMetaElementObject(Engine engine)
        {
            var obj = new HTMLMetaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMetaElement"; }
        }

        public IHtmlMetaElement RefHTMLMetaElement
        {
            get;
            set;
        }
    }
}