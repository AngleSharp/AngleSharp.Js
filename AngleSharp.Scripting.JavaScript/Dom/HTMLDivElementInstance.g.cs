namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDivElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLDivElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDivElementInstance CreateHTMLDivElementObject(EngineInstance engine)
        {
            var obj = new HTMLDivElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDivElement"; }
        }

        public IHtmlDivElement RefHTMLDivElement
        {
            get;
            set;
        }
    }
}