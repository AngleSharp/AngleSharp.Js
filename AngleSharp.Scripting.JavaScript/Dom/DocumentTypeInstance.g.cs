namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class DocumentTypeInstance : NodeInstance
    {
        readonly EngineInstance _engine;

        public DocumentTypeInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static DocumentTypeInstance CreateDocumentTypeObject(EngineInstance engine)
        {
            var obj = new DocumentTypeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "DocumentType"; }
        }

        public IDocumentType RefDocumentType
        {
            get;
            set;
        }
    }
}