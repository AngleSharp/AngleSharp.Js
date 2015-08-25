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
        readonly EngineInstance _engine;

        public HTMLLegendElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLLegendElementInstance CreateHTMLLegendElementObject(EngineInstance engine)
        {
            var obj = new HTMLLegendElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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