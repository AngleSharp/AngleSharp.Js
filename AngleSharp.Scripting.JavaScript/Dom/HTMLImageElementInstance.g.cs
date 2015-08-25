namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLImageElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLImageElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLImageElementInstance CreateHTMLImageElementObject(EngineInstance engine)
        {
            var obj = new HTMLImageElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLImageElement"; }
        }

        public IHtmlImageElement RefHTMLImageElement
        {
            get;
            set;
        }
    }
}