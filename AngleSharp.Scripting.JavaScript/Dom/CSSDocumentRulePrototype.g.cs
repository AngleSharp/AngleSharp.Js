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
            FastAddProperty("insertRule", Engine.AsValue(InsertRule), true, true, true);
            FastAddProperty("deleteRule", Engine.AsValue(DeleteRule), true, true, true);
            FastSetProperty("cssRules", Engine.AsProperty(GetCssRules));
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

        JsValue InsertRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSDocumentRuleInstance>(Fail).RefCSSDocumentRule;
            var rule = TypeConverter.ToString(arguments.At(0));
            var index = TypeConverter.ToInt32(arguments.At(1));
            return Engine.Select(reference.Insert(rule, index));
        }

        JsValue DeleteRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSDocumentRuleInstance>(Fail).RefCSSDocumentRule;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveAt(index);
            return JsValue.Undefined;
        }

        JsValue GetCssRules(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSDocumentRuleInstance>(Fail).RefCSSDocumentRule;
            return Engine.Select(reference.Rules);
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