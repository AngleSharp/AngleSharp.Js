namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSKeyframeRuleInstance : CSSRuleInstance
    {
        public CSSKeyframeRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSKeyframeRuleInstance CreateCSSKeyframeRuleObject(Engine engine)
        {
            var obj = new CSSKeyframeRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSKeyframeRule"; }
        }

        public ICssKeyframeRule RefCSSKeyframeRule
        {
            get;
            set;
        }
    }
}