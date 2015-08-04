namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDivElementInstance : HTMLElementInstance
    {
        public HTMLDivElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDivElementInstance CreateHTMLDivElementObject(Engine engine)
        {
            var obj = new HTMLDivElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDivElement"; }
        }

        public IHtmlDivElement RefHTMLDivElement
        {
            get;
            set;
        }
    }
}