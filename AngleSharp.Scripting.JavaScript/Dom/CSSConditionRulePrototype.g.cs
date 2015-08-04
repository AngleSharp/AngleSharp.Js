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

    sealed partial class CSSConditionRulePrototype : CSSConditionRuleInstance
    {
        public CSSConditionRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("insertRule", Engine.AsValue(InsertRule), true, true, true);
            FastAddProperty("deleteRule", Engine.AsValue(DeleteRule), true, true, true);
            FastSetProperty("conditionText", Engine.AsProperty(GetConditionText, SetConditionText));
            FastSetProperty("cssRules", Engine.AsProperty(GetCssRules));
        }

        public static CSSConditionRulePrototype CreatePrototypeObject(EngineInstance engine, CSSConditionRuleConstructor constructor)
        {
            var obj = new CSSConditionRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InsertRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSConditionRuleInstance>(Fail).RefCSSConditionRule;
            var rule = TypeConverter.ToString(arguments.At(0));
            var index = TypeConverter.ToInt32(arguments.At(1));
            return Engine.Select(reference.Insert(rule, index));
        }

        JsValue DeleteRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSConditionRuleInstance>(Fail).RefCSSConditionRule;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveAt(index);
            return JsValue.Undefined;
        }

        JsValue GetConditionText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSConditionRuleInstance>(Fail).RefCSSConditionRule;
            return Engine.Select(reference.ConditionText);
        }

        void SetConditionText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSConditionRuleInstance>(Fail).RefCSSConditionRule;
            var value = TypeConverter.ToString(argument);
            reference.ConditionText = value;
        }

        JsValue GetCssRules(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSConditionRuleInstance>(Fail).RefCSSConditionRule;
            return Engine.Select(reference.Rules);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSConditionRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}