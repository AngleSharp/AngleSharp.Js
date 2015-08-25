namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLLIElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLLIElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLLIElementInstance CreateHTMLLIElementObject(EngineInstance engine)
        {
            var obj = new HTMLLIElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLLIElement"; }
        }

        public IHtmlListItemElement RefHTMLLIElement
        {
            get;
            set;
        }
    }
}