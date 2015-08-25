namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLTableColElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableColElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLTableColElementInstance CreateHTMLTableColElementObject(EngineInstance engine)
        {
            var obj = new HTMLTableColElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLTableColElement"; }
        }

        public IHtmlTableColumnElement RefHTMLTableColElement
        {
            get;
            set;
        }
    }
}