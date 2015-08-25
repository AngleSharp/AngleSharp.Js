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

    sealed partial class CSSCharsetRulePrototype : CSSCharsetRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSCharsetRulePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("encoding", Engine.AsProperty(GetEncoding, SetEncoding));
        }

        public static CSSCharsetRulePrototype CreatePrototypeObject(EngineInstance engine, CSSCharsetRuleConstructor constructor)
        {
            var obj = new CSSCharsetRulePrototype(engine)
            {
                Prototype = engine.Constructors.CSSRule.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetEncoding(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSCharsetRuleInstance>(Fail).RefCSSCharsetRule;
            return _engine.GetDomNode(reference.CharacterSet);
        }

        void SetEncoding(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CSSCharsetRuleInstance>(Fail).RefCSSCharsetRule;
            var value = TypeConverter.ToString(argument);
            reference.CharacterSet = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSCharsetRule]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}