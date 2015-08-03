namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAreaElementInstance : HTMLElementInstance
    {
        public HTMLAreaElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLAreaElementInstance CreateHTMLAreaElementObject(Engine engine)
        {
            var obj = new HTMLAreaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAreaElement"; }
        }

        public IHtmlAreaElement RefHTMLAreaElement
        {
            get;
            set;
        }
    }
}