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
        public CSSImportRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSImportRuleInstance CreateCSSImportRuleObject(Engine engine)
        {
            var obj = new CSSImportRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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