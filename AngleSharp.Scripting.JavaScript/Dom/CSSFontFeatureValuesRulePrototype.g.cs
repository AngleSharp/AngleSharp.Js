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

    sealed partial class CSSFontFeatureValuesRulePrototype : CSSFontFeatureValuesRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSFontFeatureValuesRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("fontFamily", Engine.AsProperty(GetFontFamily, SetFontFamily));
        }

        public static CSSFontFeatureValuesRulePrototype CreatePrototypeObject(EngineInstance engine, CSSFontFeatureValuesRuleConstructor constructor)
        {
            var obj = new CSSFontFeatureValuesRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetFontFamily(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSFontFeatureValuesRuleInstance>(Fail).RefCSSFontFeatureValuesRule;
            return _engine.GetDomNode(reference.Family);
        }

        void SetFontFamily(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSFontFeatureValuesRuleInstance>(Fail).RefCSSFontFeatureValuesRule;
            var value = TypeConverter.ToString(argument);
            reference.Family = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSFontFeatureValuesRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}