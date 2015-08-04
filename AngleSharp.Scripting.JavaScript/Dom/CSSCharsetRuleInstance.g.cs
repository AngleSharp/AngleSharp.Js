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
        public CSSCharsetRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSCharsetRuleInstance CreateCSSCharsetRuleObject(Engine engine)
        {
            var obj = new CSSCharsetRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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