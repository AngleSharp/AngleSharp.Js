namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DocumentFragmentInstance : NodeInstance
    {
        readonly EngineInstance _engine;

        public DocumentFragmentInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static DocumentFragmentInstance CreateDocumentFragmentObject(EngineInstance engine)
        {
            var obj = new DocumentFragmentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DocumentFragment"; }
        }

        public IDocumentFragment RefDocumentFragment
        {
            get;
            set;
        }
    }
}