namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDialogElementInstance : HTMLElementInstance
    {
        public HTMLDialogElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDialogElementInstance CreateHTMLDialogElementObject(Engine engine)
        {
            var obj = new HTMLDialogElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDialogElement"; }
        }

        public IHtmlDialogElement RefHTMLDialogElement
        {
            get;
            set;
        }
    }
}