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

    sealed partial class CSSNamespaceRulePrototype : CSSNamespaceRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSNamespaceRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("namespaceURI", Engine.AsProperty(GetNamespaceURI, SetNamespaceURI));
            FastSetProperty("prefix", Engine.AsProperty(GetPrefix, SetPrefix));
        }

        public static CSSNamespaceRulePrototype CreatePrototypeObject(EngineInstance engine, CSSNamespaceRuleConstructor constructor)
        {
            var obj = new CSSNamespaceRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetNamespaceURI(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSNamespaceRuleInstance>(Fail).RefCSSNamespaceRule;
            return _engine.GetDomNode(reference.NamespaceUri);
        }

        void SetNamespaceURI(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSNamespaceRuleInstance>(Fail).RefCSSNamespaceRule;
            var value = TypeConverter.ToString(argument);
            reference.NamespaceUri = value;
        }

        JsValue GetPrefix(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSNamespaceRuleInstance>(Fail).RefCSSNamespaceRule;
            return _engine.GetDomNode(reference.Prefix);
        }

        void SetPrefix(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSNamespaceRuleInstance>(Fail).RefCSSNamespaceRule;
            var value = TypeConverter.ToString(argument);
            reference.Prefix = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSNamespaceRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}