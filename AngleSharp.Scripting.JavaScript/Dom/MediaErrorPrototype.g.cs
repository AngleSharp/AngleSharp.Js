namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MediaErrorPrototype : MediaErrorInstance
    {
        public MediaErrorPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("code", Engine.AsProperty(GetCode));
        }

        public static MediaErrorPrototype CreatePrototypeObject(EngineInstance engine, MediaErrorConstructor constructor)
        {
            var obj = new MediaErrorPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaErrorInstance>(Fail).RefMediaError;
            return Engine.Select(reference.Code);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MediaError]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}