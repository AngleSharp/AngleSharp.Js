namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSSupportsRuleInstance : CSSConditionRuleInstance
    {
        public CSSSupportsRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSSupportsRuleInstance CreateCSSSupportsRuleObject(Engine engine)
        {
            var obj = new CSSSupportsRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSSupportsRule"; }
        }

        public ICssSupportsRule RefCSSSupportsRule
        {
            get;
            set;
        }
    }
}