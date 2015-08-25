namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLDialogElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLDialogElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLDialogElementInstance CreateHTMLDialogElementObject(EngineInstance engine)
        {
            var obj = new HTMLDialogElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLDialogElement"; }
        }

        public IHtmlDialogElement RefHTMLDialogElement
        {
            get;
            set;
        }
    }
}