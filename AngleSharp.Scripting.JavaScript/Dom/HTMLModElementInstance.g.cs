namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLModElementInstance : HTMLElementInstance
    {
        public HTMLModElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLModElementInstance CreateHTMLModElementObject(Engine engine)
        {
            var obj = new HTMLModElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLModElement"; }
        }

        public IHtmlModElement RefHTMLModElement
        {
            get;
            set;
        }
    }
}