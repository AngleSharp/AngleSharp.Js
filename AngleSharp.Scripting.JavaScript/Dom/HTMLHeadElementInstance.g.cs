namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHeadElementInstance : HTMLElementInstance
    {
        public HTMLHeadElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLHeadElementInstance CreateHTMLHeadElementObject(Engine engine)
        {
            var obj = new HTMLHeadElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHeadElement"; }
        }

        public IHtmlHeadElement RefHTMLHeadElement
        {
            get;
            set;
        }
    }
}