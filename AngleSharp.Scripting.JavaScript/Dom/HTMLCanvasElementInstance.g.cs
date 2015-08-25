namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLCanvasElementInstance : HTMLElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLCanvasElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLCanvasElementInstance CreateHTMLCanvasElementObject(EngineInstance engine)
        {
            var obj = new HTMLCanvasElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLCanvasElement"; }
        }

        public IHtmlCanvasElement RefHTMLCanvasElement
        {
            get;
            set;
        }
    }
}