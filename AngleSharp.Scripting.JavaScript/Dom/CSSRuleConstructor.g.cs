namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class CSSRuleConstructor : FunctionInstance, IConstructor
    {
        public CSSRuleConstructor(Engine engine)
            : base(engine, null, null, false)
        {
            FastAddProperty("STYLE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Style), false, true, false);
            FastAddProperty("CHARSET_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Charset), false, true, false);
            FastAddProperty("IMPORT_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Import), false, true, false);
            FastAddProperty("MEDIA_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Media), false, true, false);
            FastAddProperty("FONT_FACE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.FontFace), false, true, false);
            FastAddProperty("PAGE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Page), false, true, false);
            FastAddProperty("KEYFRAMES_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Keyframes), false, true, false);
            FastAddProperty("KEYFRAME_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Keyframe), false, true, false);
            FastAddProperty("NAMESPACE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Namespace), false, true, false);
            FastAddProperty("COUNTER_STYLE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.CounterStyle), false, true, false);
            FastAddProperty("SUPPORTS_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Supports), false, true, false);
            FastAddProperty("DOCUMENT_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Document), false, true, false);
            FastAddProperty("FONT_FEATURE_VALUES_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.FontFeatureValues), false, true, false);
            FastAddProperty("VIEWPORT_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.Viewport), false, true, false);
            FastAddProperty("REGION_STYLE_RULE", (UInt32)(AngleSharp.Dom.Css.CssRuleType.RegionStyle), false, true, false);
        }

        public CSSRulePrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static CSSRuleConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new CSSRuleConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = CSSRulePrototype.CreatePrototypeObject(engine, obj);
            obj.FastAddProperty("length", 0, false, false, false);
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);
            return obj;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}