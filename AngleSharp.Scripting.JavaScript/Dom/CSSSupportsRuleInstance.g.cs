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
        readonly EngineInstance _engine;

        public CSSSupportsRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSSupportsRuleInstance CreateCSSSupportsRuleObject(EngineInstance engine)
        {
            var obj = new CSSSupportsRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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