namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSFontFaceRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSFontFaceRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSFontFaceRuleInstance CreateCSSFontFaceRuleObject(EngineInstance engine)
        {
            var obj = new CSSFontFaceRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSFontFaceRule"; }
        }

        public ICssFontFaceRule RefCSSFontFaceRule
        {
            get;
            set;
        }
    }
}