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

    sealed partial class CSSRulePrototype : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("cssText", Engine.AsProperty(GetCssText, SetCssText));
            FastSetProperty("parentRule", Engine.AsProperty(GetParentRule));
            FastSetProperty("parentStyleSheet", Engine.AsProperty(GetParentStyleSheet));
        }

        public static CSSRulePrototype CreatePrototypeObject(EngineInstance engine, CSSRuleConstructor constructor)
        {
            var obj = new CSSRulePrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSRuleInstance>(Fail).RefCSSRule;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetCssText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSRuleInstance>(Fail).RefCSSRule;
            return _engine.GetDomNode(reference.CssText);
        }

        void SetCssText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSRuleInstance>(Fail).RefCSSRule;
            var value = TypeConverter.ToString(argument);
            reference.CssText = value;
        }

        JsValue GetParentRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSRuleInstance>(Fail).RefCSSRule;
            return _engine.GetDomNode(reference.Parent);
        }


        JsValue GetParentStyleSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSRuleInstance>(Fail).RefCSSRule;
            return _engine.GetDomNode(reference.Owner);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}