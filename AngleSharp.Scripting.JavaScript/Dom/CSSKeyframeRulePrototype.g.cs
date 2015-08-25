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
        readonly EngineInstance _engine;

        public CSSKeyframeRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("keyText", Engine.AsProperty(GetKeyText, SetKeyText));
            FastSetProperty("style", Engine.AsProperty(GetStyle));
        }

        public static CSSKeyframeRulePrototype CreatePrototypeObject(EngineInstance engine, CSSKeyframeRuleConstructor constructor)
        {
            var obj = new CSSKeyframeRulePrototype(engine)
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
            return _engine.GetDomNode(reference.KeyText);
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
            return _engine.GetDomNode(reference.Style);
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