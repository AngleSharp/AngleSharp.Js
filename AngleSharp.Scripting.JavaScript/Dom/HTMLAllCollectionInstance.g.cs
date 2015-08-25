namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HTMLAllCollectionInstance : HTMLCollectionInstance
    {
        readonly EngineInstance _engine;

        public HTMLAllCollectionInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HTMLAllCollectionInstance CreateHTMLAllCollectionObject(EngineInstance engine)
        {
            var obj = new HTMLAllCollectionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HTMLAllCollection"; }
        }

        public IHtmlAllCollection RefHTMLAllCollection
        {
            get;
            set;
        }
    }
}