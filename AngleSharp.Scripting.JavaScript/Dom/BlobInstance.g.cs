namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Io;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class BlobInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public BlobInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static BlobInstance CreateBlobObject(EngineInstance engine)
        {
            var obj = new BlobInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Blob"; }
        }

        public IBlob RefBlob
        {
            get;
            set;
        }
    }
}