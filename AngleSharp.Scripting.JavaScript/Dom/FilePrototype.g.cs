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

    sealed partial class FilePrototype : FileInstance
    {
        readonly EngineInstance _engine;

        public FilePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName));
            FastSetProperty("lastModified", Engine.AsProperty(GetLastModified));
        }

        public static FilePrototype CreatePrototypeObject(EngineInstance engine, FileConstructor constructor)
        {
            var obj = new FilePrototype(engine)
            {
                Prototype = engine.Constructors.Blob.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FileInstance>(Fail).RefFile;
            return _engine.GetDomNode(reference.Name);
        }


        JsValue GetLastModified(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FileInstance>(Fail).RefFile;
            return _engine.GetDomNode(reference.LastModified);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object File]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}