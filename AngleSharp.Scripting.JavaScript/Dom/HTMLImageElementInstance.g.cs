namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLImageElementInstance : HTMLElementInstance
    {
        public HTMLImageElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLImageElementInstance CreateHTMLImageElementObject(Engine engine)
        {
            var obj = new HTMLImageElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLImageElement"; }
        }

        public IHtmlImageElement RefHTMLImageElement
        {
            get;
            set;
        }
    }
}