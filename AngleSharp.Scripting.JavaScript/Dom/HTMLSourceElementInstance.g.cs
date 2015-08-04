namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLSourceElementInstance : HTMLElementInstance
    {
        public HTMLSourceElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLSourceElementInstance CreateHTMLSourceElementObject(Engine engine)
        {
            var obj = new HTMLSourceElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLSourceElement"; }
        }

        public IHtmlSourceElement RefHTMLSourceElement
        {
            get;
            set;
        }
    }
}