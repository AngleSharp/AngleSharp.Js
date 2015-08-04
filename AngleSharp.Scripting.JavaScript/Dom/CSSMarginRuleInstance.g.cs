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
        public CSSMarginRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSMarginRuleInstance CreateCSSMarginRuleObject(Engine engine)
        {
            var obj = new CSSMarginRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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