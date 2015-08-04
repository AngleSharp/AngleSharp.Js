namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLegendElementInstance : HTMLElementInstance
    {
        public HTMLLegendElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLLegendElementInstance CreateHTMLLegendElementObject(Engine engine)
        {
            var obj = new HTMLLegendElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLegendElement"; }
        }

        public IHtmlLegendElement RefHTMLLegendElement
        {
            get;
            set;
        }
    }
}