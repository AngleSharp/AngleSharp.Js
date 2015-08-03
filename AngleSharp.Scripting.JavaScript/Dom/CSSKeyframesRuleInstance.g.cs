namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSKeyframesRuleInstance : CSSRuleInstance
    {
        public CSSKeyframesRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSKeyframesRuleInstance CreateCSSKeyframesRuleObject(Engine engine)
        {
            var obj = new CSSKeyframesRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSKeyframesRule"; }
        }

        public ICssKeyframesRule RefCSSKeyframesRule
        {
            get;
            set;
        }
    }
}