namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSMediaRuleInstance : CSSConditionRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSMediaRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSMediaRuleInstance CreateCSSMediaRuleObject(EngineInstance engine)
        {
            var obj = new CSSMediaRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSMediaRule"; }
        }

        public ICssMediaRule RefCSSMediaRule
        {
            get;
            set;
        }
    }
}