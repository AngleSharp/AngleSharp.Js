namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLUnknownElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLUnknownElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLUnknownElementInstance CreateHTMLUnknownElementObject(EngineInstance engine)
        {
            var obj = new HTMLUnknownElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLUnknownElement"; }
        }

        public IHtmlUnknownElement RefHTMLUnknownElement
        {
            get;
            set;
        }
    }
}