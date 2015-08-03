namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSStyleRuleInstance : CSSRuleInstance
    {
        public CSSStyleRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSStyleRuleInstance CreateCSSStyleRuleObject(Engine engine)
        {
            var obj = new CSSStyleRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSStyleRule"; }
        }

        public ICssStyleRule RefCSSStyleRule
        {
            get;
            set;
        }
    }
}