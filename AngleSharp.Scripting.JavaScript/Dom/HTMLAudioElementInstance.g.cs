namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAudioElementInstance : HTMLMediaElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLAudioElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLAudioElementInstance CreateHTMLAudioElementObject(EngineInstance engine)
        {
            var obj = new HTMLAudioElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAudioElement"; }
        }

        public IHtmlAudioElement RefHTMLAudioElement
        {
            get;
            set;
        }
    }
}