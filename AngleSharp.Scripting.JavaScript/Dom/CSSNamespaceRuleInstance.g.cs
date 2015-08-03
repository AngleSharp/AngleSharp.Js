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
        public CSSNamespaceRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSNamespaceRuleInstance CreateCSSNamespaceRuleObject(Engine engine)
        {
            var obj = new CSSNamespaceRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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