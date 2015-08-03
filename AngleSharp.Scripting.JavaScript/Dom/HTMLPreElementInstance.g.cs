namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLPreElementInstance : HTMLElementInstance
    {
        public HTMLPreElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLPreElementInstance CreateHTMLPreElementObject(Engine engine)
        {
            var obj = new HTMLPreElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLPreElement"; }
        }

        public IHtmlPreElement RefHTMLPreElement
        {
            get;
            set;
        }
    }
}