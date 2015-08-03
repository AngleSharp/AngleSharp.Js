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

    sealed partial class CSSKeyframesRulePrototype : CSSKeyframesRuleInstance
    {
        public CSSKeyframesRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("appendRule", Engine.AsValue(AppendRule), true, true, true);
            FastAddProperty("deleteRule", Engine.AsValue(DeleteRule), true, true, true);
            FastAddProperty("findRule", Engine.AsValue(FindRule), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("cssRules", Engine.AsProperty(GetCssRules));
        }

        public static CSSKeyframesRulePrototype CreatePrototypeObject(EngineInstance engine, CSSKeyframesRuleConstructor constructor)
        {
            var obj = new CSSKeyframesRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue AppendRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            var rule = TypeConverter.ToString(arguments.At(0));
            reference.Add(rule);
            return JsValue.Undefined;
        }

        JsValue DeleteRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            var key = TypeConverter.ToString(arguments.At(0));
            reference.Remove(key);
            return JsValue.Undefined;
        }

        JsValue FindRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            var key = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.Find(key));
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetCssRules(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSKeyframesRuleInstance>(Fail).RefCSSKeyframesRule;
            return Engine.Select(reference.Rules);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSKeyframesRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}