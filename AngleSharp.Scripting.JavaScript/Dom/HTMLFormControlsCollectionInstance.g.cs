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
        readonly EngineInstance _engine;

        public HTMLFormControlsCollectionInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLFormControlsCollectionInstance CreateHTMLFormControlsCollectionObject(EngineInstance engine)
        {
            var obj = new HTMLFormControlsCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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