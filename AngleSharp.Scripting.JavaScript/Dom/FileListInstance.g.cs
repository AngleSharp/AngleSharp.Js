namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Io;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class FileListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public FileListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static FileListInstance CreateFileListObject(EngineInstance engine)
        {
            var obj = new FileListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "FileList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefFileList[index]);
            return base.Get(propertyName);
        }


        public IFileList RefFileList
        {
            get;
            set;
        }
    }
}