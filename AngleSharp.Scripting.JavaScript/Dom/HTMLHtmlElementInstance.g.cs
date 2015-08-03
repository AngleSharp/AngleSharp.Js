namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHtmlElementInstance : HTMLElementInstance
    {
        public HTMLHtmlElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLHtmlElementInstance CreateHTMLHtmlElementObject(Engine engine)
        {
            var obj = new HTMLHtmlElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHtmlElement"; }
        }

        public IHtmlHtmlElement RefHTMLHtmlElement
        {
            get;
            set;
        }
    }
}