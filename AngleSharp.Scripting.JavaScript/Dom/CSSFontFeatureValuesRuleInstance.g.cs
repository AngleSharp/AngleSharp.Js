namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSFontFeatureValuesRuleInstance : CSSRuleInstance
    {
        public CSSFontFeatureValuesRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSFontFeatureValuesRuleInstance CreateCSSFontFeatureValuesRuleObject(Engine engine)
        {
            var obj = new CSSFontFeatureValuesRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSFontFeatureValuesRule"; }
        }

        public ICssFontFeatureValuesRule RefCSSFontFeatureValuesRule
        {
            get;
            set;
        }
    }
}