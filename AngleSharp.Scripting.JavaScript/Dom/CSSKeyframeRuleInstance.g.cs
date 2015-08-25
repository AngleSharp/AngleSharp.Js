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
        readonly EngineInstance _engine;

        public CSSKeyframeRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSKeyframeRuleInstance CreateCSSKeyframeRuleObject(EngineInstance engine)
        {
            var obj = new CSSKeyframeRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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