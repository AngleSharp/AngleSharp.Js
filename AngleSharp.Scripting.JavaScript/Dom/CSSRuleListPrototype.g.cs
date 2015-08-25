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

    sealed partial class CSSRuleListPrototype : CSSRuleListInstance
    {
        readonly EngineInstance _engine;

        public CSSRuleListPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static CSSRuleListPrototype CreatePrototypeObject(EngineInstance engine, CSSRuleListConstructor constructor)
        {
            var obj = new CSSRuleListPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSRuleListInstance>(Fail).RefCSSRuleList;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSRuleList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}