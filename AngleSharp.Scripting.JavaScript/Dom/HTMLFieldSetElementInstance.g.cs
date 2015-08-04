namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLFieldSetElementInstance : HTMLElementInstance
    {
        public HTMLFieldSetElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLFieldSetElementInstance CreateHTMLFieldSetElementObject(Engine engine)
        {
            var obj = new HTMLFieldSetElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLFieldSetElement"; }
        }

        public IHtmlFieldSetElement RefHTMLFieldSetElement
        {
            get;
            set;
        }
    }
}