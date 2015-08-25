namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMapElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMapElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLMapElementInstance CreateHTMLMapElementObject(EngineInstance engine)
        {
            var obj = new HTMLMapElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMapElement"; }
        }

        public IHtmlMapElement RefHTMLMapElement
        {
            get;
            set;
        }
    }
}