namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableColElementInstance : HTMLElementInstance
    {
        public HTMLTableColElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTableColElementInstance CreateHTMLTableColElementObject(Engine engine)
        {
            var obj = new HTMLTableColElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableColElement"; }
        }

        public IHtmlTableColumnElement RefHTMLTableColElement
        {
            get;
            set;
        }
    }
}