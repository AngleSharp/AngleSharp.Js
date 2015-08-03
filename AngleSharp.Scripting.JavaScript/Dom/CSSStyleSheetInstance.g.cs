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
        public CSSStyleSheetInstance(Engine engine)
            : base(engine)
        {
        }

        public static CSSStyleSheetInstance CreateCSSStyleSheetObject(Engine engine)
        {
            var obj = new CSSStyleSheetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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