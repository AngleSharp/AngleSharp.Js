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

    sealed partial class CSSMarginRulePrototype : CSSMarginRuleInstance
    {
        public CSSMarginRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static CSSMarginRulePrototype CreatePrototypeObject(EngineInstance engine, CSSMarginRuleConstructor constructor)
        {
            var obj = new CSSMarginRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSMarginRuleInstance>(Fail).RefCSSMarginRule;
            return Engine.Select(reference.Name);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSMarginRuleInstance>(Fail).RefCSSMarginRule;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSMarginRuleInstance>(Fail).RefCSSMarginRule;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSMarginRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}