namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class DOMStringListPrototype : DOMStringListInstance
    {
        public DOMStringListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("contains", Engine.AsValue(Contains), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static DOMStringListPrototype CreatePrototypeObject(EngineInstance engine, DOMStringListConstructor constructor)
        {
            var obj = new DOMStringListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Contains(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMStringListInstance>(Fail).RefDOMStringList;
            var entry = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.Contains(entry));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DOMStringListInstance>(Fail).RefDOMStringList;
            return Engine.Select(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DOMStringList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}