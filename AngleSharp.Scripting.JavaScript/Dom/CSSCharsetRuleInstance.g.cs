namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSCharsetRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSCharsetRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSCharsetRuleInstance CreateCSSCharsetRuleObject(EngineInstance engine)
        {
            var obj = new CSSCharsetRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSCharsetRule"; }
        }

        public ICssCharsetRule RefCSSCharsetRule
        {
            get;
            set;
        }
    }
}