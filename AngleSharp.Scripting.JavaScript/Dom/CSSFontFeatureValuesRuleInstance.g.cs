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
        readonly EngineInstance _engine;

        public CSSFontFeatureValuesRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSFontFeatureValuesRuleInstance CreateCSSFontFeatureValuesRuleObject(EngineInstance engine)
        {
            var obj = new CSSFontFeatureValuesRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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