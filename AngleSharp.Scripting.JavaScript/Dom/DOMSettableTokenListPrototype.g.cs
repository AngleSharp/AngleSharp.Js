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

    sealed partial class DOMSettableTokenListPrototype : DOMSettableTokenListInstance
    {
        public DOMSettableTokenListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
        }

        public static DOMSettableTokenListPrototype CreatePrototypeObject(EngineInstance engine, DOMSettableTokenListConstructor constructor)
        {
            var obj = new DOMSettableTokenListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.DOMTokenList.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DOMSettableTokenListInstance>(Fail).RefDOMSettableTokenList;
            return Engine.Select(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DOMSettableTokenListInstance>(Fail).RefDOMSettableTokenList;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DOMSettableTokenList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}