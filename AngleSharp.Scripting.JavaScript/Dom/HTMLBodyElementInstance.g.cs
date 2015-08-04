namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLBodyElementInstance : HTMLElementInstance
    {
        public HTMLBodyElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLBodyElementInstance CreateHTMLBodyElementObject(Engine engine)
        {
            var obj = new HTMLBodyElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLBodyElement"; }
        }

        public IHtmlBodyElement RefHTMLBodyElement
        {
            get;
            set;
        }
    }
}