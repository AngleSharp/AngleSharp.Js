namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSCounterStyleRuleInstance : CSSRuleInstance
    {
        public CSSCounterStyleRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSCounterStyleRuleInstance CreateCSSCounterStyleRuleObject(Engine engine)
        {
            var obj = new CSSCounterStyleRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSCounterStyleRule"; }
        }

        public ICssCounterStyleRule RefCSSCounterStyleRule
        {
            get;
            set;
        }
    }
}