namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLOptGroupElementInstance : HTMLElementInstance
    {
        public HTMLOptGroupElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLOptGroupElementInstance CreateHTMLOptGroupElementObject(Engine engine)
        {
            var obj = new HTMLOptGroupElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLOptGroupElement"; }
        }

        public IHtmlOptionsGroupElement RefHTMLOptGroupElement
        {
            get;
            set;
        }
    }
}