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

    sealed partial class CSSImportRulePrototype : CSSImportRuleInstance
    {
        public CSSImportRulePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("href", Engine.AsProperty(GetHref));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
            FastSetProperty("styleSheet", Engine.AsProperty(GetStyleSheet));
        }

        public static CSSImportRulePrototype CreatePrototypeObject(EngineInstance engine, CSSImportRuleConstructor constructor)
        {
            var obj = new CSSImportRulePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSImportRuleInstance>(Fail).RefCSSImportRule;
            return Engine.Select(reference.Href);
        }


        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSImportRuleInstance>(Fail).RefCSSImportRule;
            return Engine.Select(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSImportRuleInstance>(Fail).RefCSSImportRule;
            var value = TypeConverter.ToString(argument);
            reference.Media.MediaText = value;
        }

        JsValue GetStyleSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSImportRuleInstance>(Fail).RefCSSImportRule;
            return Engine.Select(reference.Sheet);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSImportRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}