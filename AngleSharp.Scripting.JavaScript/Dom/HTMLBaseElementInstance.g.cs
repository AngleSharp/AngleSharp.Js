namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLBaseElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLBaseElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLBaseElementInstance CreateHTMLBaseElementObject(EngineInstance engine)
        {
            var obj = new HTMLBaseElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLBaseElement"; }
        }

        public IHtmlBaseElement RefHTMLBaseElement
        {
            get;
            set;
        }
    }
}