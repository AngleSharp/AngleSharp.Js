namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHeadElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLHeadElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLHeadElementInstance CreateHTMLHeadElementObject(EngineInstance engine)
        {
            var obj = new HTMLHeadElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHeadElement"; }
        }

        public IHtmlHeadElement RefHTMLHeadElement
        {
            get;
            set;
        }
    }
}