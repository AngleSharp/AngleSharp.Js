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
        readonly EngineInstance _engine;

        public CSSCounterStyleRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSCounterStyleRuleInstance CreateCSSCounterStyleRuleObject(EngineInstance engine)
        {
            var obj = new CSSCounterStyleRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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