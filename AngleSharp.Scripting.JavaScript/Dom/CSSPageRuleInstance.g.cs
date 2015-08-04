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
        public CSSPageRuleInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSPageRuleInstance CreateCSSPageRuleObject(Engine engine)
        {
            var obj = new CSSPageRuleInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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