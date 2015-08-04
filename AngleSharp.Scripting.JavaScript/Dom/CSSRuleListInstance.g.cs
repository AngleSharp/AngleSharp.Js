namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSRuleListInstance : ObjectInstance
    {
        public CSSRuleListInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSRuleListInstance CreateCSSRuleListObject(Engine engine)
        {
            var obj = new CSSRuleListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSRuleList"; }
        }

        public ICssRuleList RefCSSRuleList
        {
            get;
            set;
        }
    }
}