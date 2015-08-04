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
        public HTMLVideoElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLVideoElementInstance CreateHTMLVideoElementObject(Engine engine)
        {
            var obj = new HTMLVideoElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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