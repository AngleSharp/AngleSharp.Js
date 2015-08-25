namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLVideoElementInstance : HTMLMediaElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLVideoElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLVideoElementInstance CreateHTMLVideoElementObject(EngineInstance engine)
        {
            var obj = new HTMLVideoElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLVideoElement"; }
        }

        public IHtmlVideoElement RefHTMLVideoElement
        {
            get;
            set;
        }
    }
}