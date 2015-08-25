namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSNamespaceRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSNamespaceRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSNamespaceRuleInstance CreateCSSNamespaceRuleObject(EngineInstance engine)
        {
            var obj = new CSSNamespaceRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSNamespaceRule"; }
        }

        public ICssNamespaceRule RefCSSNamespaceRule
        {
            get;
            set;
        }
    }
}