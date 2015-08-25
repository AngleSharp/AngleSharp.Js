namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOutputElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLOutputElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLOutputElementInstance CreateHTMLOutputElementObject(EngineInstance engine)
        {
            var obj = new HTMLOutputElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOutputElement"; }
        }

        public IHtmlOutputElement RefHTMLOutputElement
        {
            get;
            set;
        }
    }
}