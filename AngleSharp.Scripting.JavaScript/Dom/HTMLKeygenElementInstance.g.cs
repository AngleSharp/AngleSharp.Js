namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLKeygenElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLKeygenElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLKeygenElementInstance CreateHTMLKeygenElementObject(EngineInstance engine)
        {
            var obj = new HTMLKeygenElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLKeygenElement"; }
        }

        public IHtmlKeygenElement RefHTMLKeygenElement
        {
            get;
            set;
        }
    }
}