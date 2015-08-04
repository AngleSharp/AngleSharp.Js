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

    sealed partial class BlobPrototype : BlobInstance
    {
        public BlobPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("slice", Engine.AsValue(Slice), true, true, true);
            FastAddProperty("close", Engine.AsValue(Close), true, true, true);
            FastSetProperty("size", Engine.AsProperty(GetSize));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("isClosed", Engine.AsProperty(GetIsClosed));
        }

        public static BlobPrototype CreatePrototypeObject(EngineInstance engine, BlobConstructor constructor)
        {
            var obj = new BlobPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Slice(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<BlobInstance>(Fail).RefBlob;
            var start = TypeConverter.ToInt32(arguments.At(0));
            var end = TypeConverter.ToInt32(arguments.At(1));
            var contentType = TypeConverter.ToString(arguments.At(2));
            return Engine.Select(reference.Slice(start, end, contentType));
        }

        JsValue Close(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<BlobInstance>(Fail).RefBlob;
            reference.Close();
            return JsValue.Undefined;
        }

        JsValue GetSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<BlobInstance>(Fail).RefBlob;
            return Engine.Select(reference.Length);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<BlobInstance>(Fail).RefBlob;
            return Engine.Select(reference.Type);
        }


        JsValue GetIsClosed(JsValue thisObj)
        {
            var reference = thisObj.TryCast<BlobInstance>(Fail).RefBlob;
            return Engine.Select(reference.IsClosed);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Blob]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}