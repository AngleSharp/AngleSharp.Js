namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDetailsElementInstance : HTMLElementInstance
    {
        public HTMLDetailsElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLDetailsElementInstance CreateHTMLDetailsElementObject(Engine engine)
        {
            var obj = new HTMLDetailsElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDetailsElement"; }
        }

        public IHtmlDetailsElement RefHTMLDetailsElement
        {
            get;
            set;
        }
    }
}