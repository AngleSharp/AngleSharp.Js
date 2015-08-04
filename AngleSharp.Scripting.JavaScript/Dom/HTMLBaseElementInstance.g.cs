namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLBaseElementInstance : HTMLElementInstance
    {
        public HTMLBaseElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLBaseElementInstance CreateHTMLBaseElementObject(Engine engine)
        {
            var obj = new HTMLBaseElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLBaseElement"; }
        }

        public IHtmlBaseElement RefHTMLBaseElement
        {
            get;
            set;
        }
    }
}