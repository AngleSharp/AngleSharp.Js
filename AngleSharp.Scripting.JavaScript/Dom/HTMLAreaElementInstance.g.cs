namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAreaElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLAreaElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLAreaElementInstance CreateHTMLAreaElementObject(EngineInstance engine)
        {
            var obj = new HTMLAreaElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAreaElement"; }
        }

        public IHtmlAreaElement RefHTMLAreaElement
        {
            get;
            set;
        }
    }
}