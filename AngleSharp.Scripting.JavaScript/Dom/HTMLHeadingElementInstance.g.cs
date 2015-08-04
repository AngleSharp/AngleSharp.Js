namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHeadingElementInstance : HTMLElementInstance
    {
        public HTMLHeadingElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLHeadingElementInstance CreateHTMLHeadingElementObject(Engine engine)
        {
            var obj = new HTMLHeadingElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHeadingElement"; }
        }

        public IHtmlHeadingElement RefHTMLHeadingElement
        {
            get;
            set;
        }
    }
}