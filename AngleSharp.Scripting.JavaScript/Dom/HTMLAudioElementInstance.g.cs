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
        public HTMLAudioElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLAudioElementInstance CreateHTMLAudioElementObject(Engine engine)
        {
            var obj = new HTMLAudioElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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