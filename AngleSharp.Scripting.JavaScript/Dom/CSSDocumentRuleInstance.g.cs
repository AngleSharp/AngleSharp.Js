namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSDocumentRuleInstance : CSSConditionRuleInstance
    {
        readonly EngineInstance _engine;

        public CSSDocumentRuleInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSDocumentRuleInstance CreateCSSDocumentRuleObject(EngineInstance engine)
        {
            var obj = new CSSDocumentRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSDocumentRule"; }
        }

        public ICssDocumentRule RefCSSDocumentRule
        {
            get;
            set;
        }
    }
}