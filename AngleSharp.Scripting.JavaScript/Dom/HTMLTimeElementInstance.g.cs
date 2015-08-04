namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTimeElementInstance : HTMLElementInstance
    {
        public HTMLTimeElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLTimeElementInstance CreateHTMLTimeElementObject(Engine engine)
        {
            var obj = new HTMLTimeElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTimeElement"; }
        }

        public IHtmlTimeElement RefHTMLTimeElement
        {
            get;
            set;
        }
    }
}