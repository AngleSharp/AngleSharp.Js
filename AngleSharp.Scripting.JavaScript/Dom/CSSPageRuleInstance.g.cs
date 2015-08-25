namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSPageRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSPageRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSPageRuleInstance CreateCSSPageRuleObject(EngineInstance engine)
        {
            var obj = new CSSPageRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSPageRule"; }
        }

        public ICssPageRule RefCSSPageRule
        {
            get;
            set;
        }
    }
}