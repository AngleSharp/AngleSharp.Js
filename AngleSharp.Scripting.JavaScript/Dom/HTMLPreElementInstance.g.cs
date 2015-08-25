namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLPreElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLPreElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLPreElementInstance CreateHTMLPreElementObject(EngineInstance engine)
        {
            var obj = new HTMLPreElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLPreElement"; }
        }

        public IHtmlPreElement RefHTMLPreElement
        {
            get;
            set;
        }
    }
}