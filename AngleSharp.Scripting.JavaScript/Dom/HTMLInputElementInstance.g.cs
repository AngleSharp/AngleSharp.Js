namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLInputElementInstance : HTMLElementInstance
    {
        public HTMLInputElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLInputElementInstance CreateHTMLInputElementObject(Engine engine)
        {
            var obj = new HTMLInputElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLInputElement"; }
        }

        public IHtmlInputElement RefHTMLInputElement
        {
            get;
            set;
        }
    }
}