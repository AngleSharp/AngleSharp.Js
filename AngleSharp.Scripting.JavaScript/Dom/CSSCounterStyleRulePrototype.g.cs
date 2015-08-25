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

    sealed partial class CSSCounterStyleRulePrototype : CSSCounterStyleRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSCounterStyleRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("system", Engine.AsProperty(GetSystem, SetSystem));
            FastSetProperty("symbols", Engine.AsProperty(GetSymbols, SetSymbols));
            FastSetProperty("additiveSymbols", Engine.AsProperty(GetAdditiveSymbols, SetAdditiveSymbols));
            FastSetProperty("negative", Engine.AsProperty(GetNegative, SetNegative));
            FastSetProperty("prefix", Engine.AsProperty(GetPrefix, SetPrefix));
            FastSetProperty("suffix", Engine.AsProperty(GetSuffix, SetSuffix));
            FastSetProperty("range", Engine.AsProperty(GetRange, SetRange));
            FastSetProperty("pad", Engine.AsProperty(GetPad, SetPad));
            FastSetProperty("speakAs", Engine.AsProperty(GetSpeakAs, SetSpeakAs));
            FastSetProperty("fallback", Engine.AsProperty(GetFallback, SetFallback));
        }

        public static CSSCounterStyleRulePrototype CreatePrototypeObject(EngineInstance engine, CSSCounterStyleRuleConstructor constructor)
        {
            var obj = new CSSCounterStyleRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetSystem(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.System);
        }

        void SetSystem(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.System = value;
        }

        JsValue GetSymbols(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Symbols);
        }

        void SetSymbols(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Symbols = value;
        }

        JsValue GetAdditiveSymbols(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.AdditiveSymbols);
        }

        void SetAdditiveSymbols(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.AdditiveSymbols = value;
        }

        JsValue GetNegative(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Negative);
        }

        void SetNegative(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Negative = value;
        }

        JsValue GetPrefix(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Prefix);
        }

        void SetPrefix(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Prefix = value;
        }

        JsValue GetSuffix(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Suffix);
        }

        void SetSuffix(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Suffix = value;
        }

        JsValue GetRange(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Range);
        }

        void SetRange(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Range = value;
        }

        JsValue GetPad(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Pad);
        }

        void SetPad(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Pad = value;
        }

        JsValue GetSpeakAs(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.SpeakAs);
        }

        void SetSpeakAs(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.SpeakAs = value;
        }

        JsValue GetFallback(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            return _engine.GetDomNode(reference.Fallback);
        }

        void SetFallback(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCounterStyleRuleInstance>(Fail).RefCSSCounterStyleRule;
            var value = TypeConverter.ToString(argument);
            reference.Fallback = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSCounterStyleRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}