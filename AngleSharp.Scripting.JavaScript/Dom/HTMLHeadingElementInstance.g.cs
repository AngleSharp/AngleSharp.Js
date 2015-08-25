namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHeadingElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLHeadingElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLHeadingElementInstance CreateHTMLHeadingElementObject(EngineInstance engine)
        {
            var obj = new HTMLHeadingElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHeadingElement"; }
        }

        public IHtmlHeadingElement RefHTMLHeadingElement
        {
            get;
            set;
        }
    }
}