namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class StyleSheetListInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public StyleSheetListInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static StyleSheetListInstance CreateStyleSheetListObject(EngineInstance engine)
        {
            var obj = new StyleSheetListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "StyleSheetList"; }
        }
        
        public override JsValue Get(String propertyName)
        {
            var index = default(Int32);

            if (Int32.TryParse(propertyName, out index))
                return _engine.GetDomNode(RefStyleSheetList[index]);
            return base.Get(propertyName);
        }


        public IStyleSheetList RefStyleSheetList
        {
            get;
            set;
        }
    }
}