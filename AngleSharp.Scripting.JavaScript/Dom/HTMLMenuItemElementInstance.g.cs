namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMenuItemElementInstance : HTMLElementInstance
    {
        public HTMLMenuItemElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLMenuItemElementInstance CreateHTMLMenuItemElementObject(Engine engine)
        {
            var obj = new HTMLMenuItemElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMenuItemElement"; }
        }

        public IHtmlMenuItemElement RefHTMLMenuItemElement
        {
            get;
            set;
        }
    }
}