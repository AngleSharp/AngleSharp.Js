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
        readonly EngineInstance _engine;

        public CSSStyleRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSStyleRuleInstance CreateCSSStyleRuleObject(EngineInstance engine)
        {
            var obj = new CSSStyleRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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