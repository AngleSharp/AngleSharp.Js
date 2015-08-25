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
        readonly EngineInstance _engine;

        public CSSKeyframesRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSKeyframesRuleInstance CreateCSSKeyframesRuleObject(EngineInstance engine)
        {
            var obj = new CSSKeyframesRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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