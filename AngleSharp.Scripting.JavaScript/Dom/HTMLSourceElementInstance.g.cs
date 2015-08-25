namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLSourceElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLSourceElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLSourceElementInstance CreateHTMLSourceElementObject(EngineInstance engine)
        {
            var obj = new HTMLSourceElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLSourceElement"; }
        }

        public IHtmlSourceElement RefHTMLSourceElement
        {
            get;
            set;
        }
    }
}