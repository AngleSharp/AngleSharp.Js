namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSStyleSheetInstance : StyleSheetInstance
    {
        readonly EngineInstance _engine;

        public CSSStyleSheetInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CSSStyleSheetInstance CreateCSSStyleSheetObject(EngineInstance engine)
        {
            var obj = new CSSStyleSheetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSStyleSheet"; }
        }

        public ICssStyleSheet RefCSSStyleSheet
        {
            get;
            set;
        }
    }
}