namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Io;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class FileListPrototype : FileListInstance
    {
        readonly EngineInstance _engine;

        public FileListPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static FileListPrototype CreatePrototypeObject(EngineInstance engine, FileListConstructor constructor)
        {
            var obj = new FileListPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FileListInstance>(Fail).RefFileList;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object FileList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}