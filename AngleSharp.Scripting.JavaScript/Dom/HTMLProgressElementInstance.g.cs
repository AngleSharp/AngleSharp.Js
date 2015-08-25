namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLProgressElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLProgressElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLProgressElementInstance CreateHTMLProgressElementObject(EngineInstance engine)
        {
            var obj = new HTMLProgressElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLProgressElement"; }
        }

        public IHtmlProgressElement RefHTMLProgressElement
        {
            get;
            set;
        }
    }
}