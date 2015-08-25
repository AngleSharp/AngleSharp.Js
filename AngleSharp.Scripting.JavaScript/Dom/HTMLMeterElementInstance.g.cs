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
        readonly EngineInstance _engine;

        public HTMLMeterElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLMeterElementInstance CreateHTMLMeterElementObject(EngineInstance engine)
        {
            var obj = new HTMLMeterElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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