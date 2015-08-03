namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLParamElementInstance : HTMLElementInstance
    {
        public HTMLParamElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLParamElementInstance CreateHTMLParamElementObject(Engine engine)
        {
            var obj = new HTMLParamElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLParamElement"; }
        }

        public IHtmlParamElement RefHTMLParamElement
        {
            get;
            set;
        }
    }
}