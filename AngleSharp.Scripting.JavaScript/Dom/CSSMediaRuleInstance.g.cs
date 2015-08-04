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
        public CSSMediaRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSMediaRuleInstance CreateCSSMediaRuleObject(Engine engine)
        {
            var obj = new CSSMediaRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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