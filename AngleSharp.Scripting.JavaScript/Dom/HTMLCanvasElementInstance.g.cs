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
        public HTMLCanvasElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLCanvasElementInstance CreateHTMLCanvasElementObject(Engine engine)
        {
            var obj = new HTMLCanvasElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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