namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class CSSDocumentRulePrototype : CSSDocumentRuleInstance
    {
        public CSSDocumentRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static CSSDocumentRulePrototype CreatePrototypeObject(EngineInstance engine, CSSDocumentRuleConstructor constructor)
        {
            var obj = new CSSDocumentRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSConditionRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSDocumentRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}