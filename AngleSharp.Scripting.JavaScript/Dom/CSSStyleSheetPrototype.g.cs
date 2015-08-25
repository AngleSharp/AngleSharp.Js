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

    sealed partial class CSSStyleSheetPrototype : CSSStyleSheetInstance
    {
        readonly EngineInstance _engine;

        public CSSStyleSheetPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("insertRule", Engine.AsValue(InsertRule), true, true, true);
            FastAddProperty("deleteRule", Engine.AsValue(DeleteRule), true, true, true);
            FastSetProperty("ownerRule", Engine.AsProperty(GetOwnerRule));
            FastSetProperty("cssRules", Engine.AsProperty(GetCssRules));
        }

        public static CSSStyleSheetPrototype CreatePrototypeObject(EngineInstance engine, CSSStyleSheetConstructor constructor)
        {
            var obj = new CSSStyleSheetPrototype(engine)
            {
                Prototype = engine.Constructors.StyleSheet.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InsertRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleSheetInstance>(Fail).RefCSSStyleSheet;
            var rule = TypeConverter.ToString(arguments.At(0));
            var index = TypeConverter.ToInt32(arguments.At(1));
            return _engine.GetDomNode(reference.Insert(rule, index));
        }

        JsValue DeleteRule(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CSSStyleSheetInstance>(Fail).RefCSSStyleSheet;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveAt(index);
            return JsValue.Undefined;
        }

        JsValue GetOwnerRule(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleSheetInstance>(Fail).RefCSSStyleSheet;
            return _engine.GetDomNode(reference.OwnerRule);
        }


        JsValue GetCssRules(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CSSStyleSheetInstance>(Fail).RefCSSStyleSheet;
            return _engine.GetDomNode(reference.Rules);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CSSStyleSheet]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}