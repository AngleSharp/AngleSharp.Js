namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSConditionRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSConditionRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSConditionRuleInstance CreateCSSConditionRuleObject(EngineInstance engine)
        {
            var obj = new CSSConditionRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSConditionRule"; }
        }

        public ICssConditionRule RefCSSConditionRule
        {
            get;
            set;
        }
    }
}