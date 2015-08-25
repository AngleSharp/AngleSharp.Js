namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CSSStyleDeclarationInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public CSSStyleDeclarationInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static CSSStyleDeclarationInstance CreateCSSStyleDeclarationObject(EngineInstance engine)
        {
            var obj = new CSSStyleDeclarationInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CSSStyleDeclaration"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefCSSStyleDeclaration[index]);
            return base.Get(propertyName);
        }


        public ICssStyleDeclaration RefCSSStyleDeclaration
        {
            get;
            set;
        }
    }
}