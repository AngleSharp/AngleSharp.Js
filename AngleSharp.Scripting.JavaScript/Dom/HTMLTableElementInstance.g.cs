namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableElementInstance : HTMLElementInstance
    {
        public HTMLTableElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableElementInstance CreateHTMLTableElementObject(Engine engine)
        {
            var obj = new HTMLTableElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableElement"; }
        }

        public IHtmlTableElement RefHTMLTableElement
        {
            get;
            set;
        }
    }
}