namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSRuleInstance : ObjectInstance
    {
        public CSSRuleInstance(Engine engine)
            : base(engine)
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

        public static CSSRuleInstance CreateCSSRuleObject(Engine engine)
        {
            var obj = new CSSRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSRule"; }
        }

        public ICssRule RefCSSRule
        {
            get;
            set;
        }
    }
}