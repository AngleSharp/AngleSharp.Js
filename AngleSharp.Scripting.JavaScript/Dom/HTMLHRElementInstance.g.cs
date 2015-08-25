namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHRElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLHRElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLHRElementInstance CreateHTMLHRElementObject(EngineInstance engine)
        {
            var obj = new HTMLHRElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHRElement"; }
        }

        public IHtmlHrElement RefHTMLHRElement
        {
            get;
            set;
        }
    }
}