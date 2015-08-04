namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLHRElementInstance : HTMLElementInstance
    {
        public HTMLHRElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLHRElementInstance CreateHTMLHRElementObject(Engine engine)
        {
            var obj = new HTMLHRElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLHRElement"; }
        }

        public IHtmlHrElement RefHTMLHRElement
        {
            get;
            set;
        }
    }
}