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

    sealed partial class CSSStyleRulePrototype : CSSStyleRuleInstance
    {
        public CSSStyleRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("selectorText", Engine.AsProperty(GetSelectorText, SetSelectorText));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static CSSStyleRulePrototype CreatePrototypeObject(EngineInstance engine, CSSStyleRuleConstructor constructor)
        {
            var obj = new CSSStyleRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSelectorText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleRuleInstance>(Fail).RefCSSStyleRule;
            return Engine.Select(reference.SelectorText);
        }

        void SetSelectorText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleRuleInstance>(Fail).RefCSSStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.SelectorText = value;
        }

        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleRuleInstance>(Fail).RefCSSStyleRule;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSStyleRuleInstance>(Fail).RefCSSStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSStyleRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}