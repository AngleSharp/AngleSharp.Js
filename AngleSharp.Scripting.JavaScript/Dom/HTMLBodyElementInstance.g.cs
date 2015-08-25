namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLBodyElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLBodyElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLBodyElementInstance CreateHTMLBodyElementObject(EngineInstance engine)
        {
            var obj = new HTMLBodyElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLBodyElement"; }
        }

        public IHtmlBodyElement RefHTMLBodyElement
        {
            get;
            set;
        }
    }
}