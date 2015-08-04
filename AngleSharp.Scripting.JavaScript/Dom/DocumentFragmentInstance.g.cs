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
        public DocumentFragmentInstance(Engine engine)
            : base(engine)
        {
        }

        public static DocumentFragmentInstance CreateDocumentFragmentObject(Engine engine)
        {
            var obj = new DocumentFragmentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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