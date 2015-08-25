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
        readonly EngineInstance _engine;

        public HTMLDetailsElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDetailsElementInstance CreateHTMLDetailsElementObject(EngineInstance engine)
        {
            var obj = new HTMLDetailsElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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