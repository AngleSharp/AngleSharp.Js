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

    sealed partial class CSSMediaRulePrototype : CSSMediaRuleInstance
    {
        public CSSMediaRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
        }

        public static CSSMediaRulePrototype CreatePrototypeObject(EngineInstance engine, CSSMediaRuleConstructor constructor)
        {
            var obj = new CSSMediaRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSConditionRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSMediaRuleInstance>(Fail).RefCSSMediaRule;
            return Engine.Select(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSMediaRuleInstance>(Fail).RefCSSMediaRule;
            var value = TypeConverter.ToString(argument);
            reference.Media.MediaText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSMediaRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}