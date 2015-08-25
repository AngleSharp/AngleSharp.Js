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

    sealed partial class CSSPageRulePrototype : CSSPageRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSPageRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("selectorText", Engine.AsProperty(GetSelectorText, SetSelectorText));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static CSSPageRulePrototype CreatePrototypeObject(EngineInstance engine, CSSPageRuleConstructor constructor)
        {
            var obj = new CSSPageRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSelectorText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSPageRuleInstance>(Fail).RefCSSPageRule;
            return _engine.GetDomNode(reference.SelectorText);
        }

        void SetSelectorText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSPageRuleInstance>(Fail).RefCSSPageRule;
            var value = TypeConverter.ToString(argument);
            reference.SelectorText = value;
        }

        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSPageRuleInstance>(Fail).RefCSSPageRule;
            return _engine.GetDomNode(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSPageRuleInstance>(Fail).RefCSSPageRule;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSPageRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}