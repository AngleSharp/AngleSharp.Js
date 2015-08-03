namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLProgressElementInstance : HTMLElementInstance
    {
        public HTMLProgressElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLProgressElementInstance CreateHTMLProgressElementObject(Engine engine)
        {
            var obj = new HTMLProgressElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLProgressElement"; }
        }

        public IHtmlProgressElement RefHTMLProgressElement
        {
            get;
            set;
        }
    }
}