namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLabelElementInstance : HTMLElementInstance
    {
        public HTMLLabelElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLLabelElementInstance CreateHTMLLabelElementObject(Engine engine)
        {
            var obj = new HTMLLabelElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLabelElement"; }
        }

        public IHtmlLabelElement RefHTMLLabelElement
        {
            get;
            set;
        }
    }
}