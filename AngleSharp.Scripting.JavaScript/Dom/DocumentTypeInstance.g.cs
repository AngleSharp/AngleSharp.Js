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
        public DocumentTypeInstance(Engine engine)
            : base(engine)
        {
        }

        public static DocumentTypeInstance CreateDocumentTypeObject(Engine engine)
        {
            var obj = new DocumentTypeInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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