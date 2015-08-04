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

    sealed partial class CSSKeyframeRulePrototype : CSSKeyframeRuleInstance
    {
        public CSSKeyframeRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("keyText", Engine.AsProperty(GetKeyText, SetKeyText));
            FastSetProperty("style", Engine.AsProperty(GetStyle));
        }

        public static CSSKeyframeRulePrototype CreatePrototypeObject(EngineInstance engine, CSSKeyframeRuleConstructor constructor)
        {
            var obj = new CSSKeyframeRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetKeyText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSKeyframeRuleInstance>(Fail).RefCSSKeyframeRule;
            return Engine.Select(reference.KeyText);
        }

        void SetKeyText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSKeyframeRuleInstance>(Fail).RefCSSKeyframeRule;
            var value = TypeConverter.ToString(argument);
            reference.KeyText = value;
        }

        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSKeyframeRuleInstance>(Fail).RefCSSKeyframeRule;
            return Engine.Select(reference.Style);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSKeyframeRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}