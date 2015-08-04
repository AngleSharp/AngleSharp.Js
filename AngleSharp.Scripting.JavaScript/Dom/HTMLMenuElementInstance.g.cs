namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMenuElementInstance : HTMLElementInstance
    {
        public HTMLMenuElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLMenuElementInstance CreateHTMLMenuElementObject(Engine engine)
        {
            var obj = new HTMLMenuElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMenuElement"; }
        }

        public IHtmlMenuElement RefHTMLMenuElement
        {
            get;
            set;
        }
    }
}