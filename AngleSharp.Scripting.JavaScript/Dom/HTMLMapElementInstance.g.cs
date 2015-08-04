namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLMapElementInstance : HTMLElementInstance
    {
        public HTMLMapElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLMapElementInstance CreateHTMLMapElementObject(Engine engine)
        {
            var obj = new HTMLMapElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLMapElement"; }
        }

        public IHtmlMapElement RefHTMLMapElement
        {
            get;
            set;
        }
    }
}