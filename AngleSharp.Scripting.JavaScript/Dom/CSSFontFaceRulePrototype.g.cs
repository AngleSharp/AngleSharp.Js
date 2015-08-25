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

    sealed partial class CSSFontFaceRulePrototype : CSSFontFaceRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSFontFaceRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("family", Engine.AsProperty(GetFamily, SetFamily));
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
            FastSetProperty("weight", Engine.AsProperty(GetWeight, SetWeight));
            FastSetProperty("stretch", Engine.AsProperty(GetStretch, SetStretch));
            FastSetProperty("unicodeRange", Engine.AsProperty(GetUnicodeRange, SetUnicodeRange));
            FastSetProperty("variant", Engine.AsProperty(GetVariant, SetVariant));
            FastSetProperty("featureSettings", Engine.AsProperty(GetFeatureSettings, SetFeatureSettings));
        }

        public static CSSFontFaceRulePrototype CreatePrototypeObject(EngineInstance engine, CSSFontFaceRuleConstructor constructor)
        {
            var obj = new CSSFontFaceRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetFamily(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Family);
        }

        void SetFamily(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Family = value;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Style = value;
        }

        JsValue GetWeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Weight);
        }

        void SetWeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Weight = value;
        }

        JsValue GetStretch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Stretch);
        }

        void SetStretch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Stretch = value;
        }

        JsValue GetUnicodeRange(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Range);
        }

        void SetUnicodeRange(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Range = value;
        }

        JsValue GetVariant(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Variant);
        }

        void SetVariant(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Variant = value;
        }

        JsValue GetFeatureSettings(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            return _engine.GetDomNode(reference.Features);
        }

        void SetFeatureSettings(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFaceRuleInstance>(Fail).RefCSSFontFaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Features = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSFontFaceRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}