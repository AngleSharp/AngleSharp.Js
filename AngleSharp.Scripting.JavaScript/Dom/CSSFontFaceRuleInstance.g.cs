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
        public CSSFontFaceRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSFontFaceRuleInstance CreateCSSFontFaceRuleObject(Engine engine)
        {
            var obj = new CSSFontFaceRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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