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
        readonly EngineInstance _engine;

        public FileInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static FileInstance CreateFileObject(EngineInstance engine)
        {
            var obj = new FileInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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