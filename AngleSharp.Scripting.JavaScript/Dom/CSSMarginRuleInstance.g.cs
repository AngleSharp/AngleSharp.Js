namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSMarginRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSMarginRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSMarginRuleInstance CreateCSSMarginRuleObject(EngineInstance engine)
        {
            var obj = new CSSMarginRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSMarginRule"; }
        }

        public ICssMarginRule RefCSSMarginRule
        {
            get;
            set;
        }
    }
}