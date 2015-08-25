namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDataElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLDataElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDataElementInstance CreateHTMLDataElementObject(EngineInstance engine)
        {
            var obj = new HTMLDataElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDataElement"; }
        }

        public IHtmlDataElement RefHTMLDataElement
        {
            get;
            set;
        }
    }
}