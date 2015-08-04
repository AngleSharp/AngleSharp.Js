namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLCommandElementInstance : HTMLElementInstance
    {
        public HTMLCommandElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLCommandElementInstance CreateHTMLCommandElementObject(Engine engine)
        {
            var obj = new HTMLCommandElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLCommandElement"; }
        }

        public IHtmlCommandElement RefHTMLCommandElement
        {
            get;
            set;
        }
    }
}