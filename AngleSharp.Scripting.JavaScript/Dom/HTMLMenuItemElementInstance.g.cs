namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMenuItemElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMenuItemElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLMenuItemElementInstance CreateHTMLMenuItemElementObject(EngineInstance engine)
        {
            var obj = new HTMLMenuItemElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMenuItemElement"; }
        }

        public IHtmlMenuItemElement RefHTMLMenuItemElement
        {
            get;
            set;
        }
    }
}