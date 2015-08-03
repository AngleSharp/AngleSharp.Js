namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOutputElementInstance : HTMLElementInstance
    {
        public HTMLOutputElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLOutputElementInstance CreateHTMLOutputElementObject(Engine engine)
        {
            var obj = new HTMLOutputElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOutputElement"; }
        }

        public IHtmlOutputElement RefHTMLOutputElement
        {
            get;
            set;
        }
    }
}