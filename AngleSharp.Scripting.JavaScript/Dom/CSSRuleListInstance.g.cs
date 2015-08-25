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
        readonly EngineInstance _engine;

        public CSSRuleListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static CSSRuleListInstance CreateCSSRuleListObject(EngineInstance engine)
        {
            var obj = new CSSRuleListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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