namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLParamElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLParamElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLParamElementInstance CreateHTMLParamElementObject(EngineInstance engine)
        {
            var obj = new HTMLParamElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLParamElement"; }
        }

        public IHtmlParamElement RefHTMLParamElement
        {
            get;
            set;
        }
    }
}