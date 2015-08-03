namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Io;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class FileInstance : BlobInstance
    {
        public FileInstance(Engine engine)
            : base(engine)
        {
        }

        public static FileInstance CreateFileObject(Engine engine)
        {
            var obj = new FileInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "File"; }
        }

        public IFile RefFile
        {
            get;
            set;
        }
    }
}