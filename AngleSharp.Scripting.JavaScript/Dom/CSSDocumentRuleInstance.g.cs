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
        public CSSDocumentRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSDocumentRuleInstance CreateCSSDocumentRuleObject(Engine engine)
        {
            var obj = new CSSDocumentRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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