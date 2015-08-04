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
        public CSSConditionRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSConditionRuleInstance CreateCSSConditionRuleObject(Engine engine)
        {
            var obj = new CSSConditionRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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