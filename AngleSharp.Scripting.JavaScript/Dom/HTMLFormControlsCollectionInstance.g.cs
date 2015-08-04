namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLFormControlsCollectionInstance : HTMLCollectionInstance
    {
        public HTMLFormControlsCollectionInstance(Engine engine)
            : base(engine)
        {
        }

        public static HTMLFormControlsCollectionInstance CreateHTMLFormControlsCollectionObject(Engine engine)
        {
            var obj = new HTMLFormControlsCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLFormControlsCollection"; }
        }

        public IHtmlFormControlsCollection RefHTMLFormControlsCollection
        {
            get;
            set;
        }
    }
}