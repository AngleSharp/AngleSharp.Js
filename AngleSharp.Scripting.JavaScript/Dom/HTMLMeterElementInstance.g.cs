namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMeterElementInstance : HTMLElementInstance
    {
        public HTMLMeterElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLMeterElementInstance CreateHTMLMeterElementObject(Engine engine)
        {
            var obj = new HTMLMeterElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMeterElement"; }
        }

        public IHtmlMeterElement RefHTMLMeterElement
        {
            get;
            set;
        }
    }
}