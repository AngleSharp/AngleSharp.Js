namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSImportRuleInstance : CSSRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSImportRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSImportRuleInstance CreateCSSImportRuleObject(EngineInstance engine)
        {
            var obj = new CSSImportRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSImportRule"; }
        }

        public ICssImportRule RefCSSImportRule
        {
            get;
            set;
        }
    }
}