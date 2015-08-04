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
        public BlobInstance(Engine engine)
            : base(engine)
        {
        }

        public static BlobInstance CreateBlobObject(Engine engine)
        {
            var obj = new BlobInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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