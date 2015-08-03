namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLButtonElementInstance : HTMLElementInstance
    {
        public HTMLButtonElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLButtonElementInstance CreateHTMLButtonElementObject(Engine engine)
        {
            var obj = new HTMLButtonElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLButtonElement"; }
        }

        public IHtmlButtonElement RefHTMLButtonElement
        {
            get;
            set;
        }
    }
}