namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLKeygenElementInstance : HTMLElementInstance
    {
        public HTMLKeygenElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLKeygenElementInstance CreateHTMLKeygenElementObject(Engine engine)
        {
            var obj = new HTMLKeygenElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLKeygenElement"; }
        }

        public IHtmlKeygenElement RefHTMLKeygenElement
        {
            get;
            set;
        }
    }
}