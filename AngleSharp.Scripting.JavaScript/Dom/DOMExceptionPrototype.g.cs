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

    sealed partial class DOMExceptionPrototype : DOMExceptionInstance
    {
        readonly EngineInstance _engine;

        public DOMExceptionPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("code", Engine.AsProperty(GetCode));
        }

        public static DOMExceptionPrototype CreatePrototypeObject(EngineInstance engine, DOMExceptionConstructor constructor)
        {
            var obj = new DOMExceptionPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DOMExceptionInstance>(Fail).RefDOMException;
            return _engine.GetDomNode(reference.Code);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DOMException]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}