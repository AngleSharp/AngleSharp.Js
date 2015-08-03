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
        public StyleSheetListInstance(Engine engine)
            : base(engine)
        {
        }

        public static StyleSheetListInstance CreateStyleSheetListObject(Engine engine)
        {
            var obj = new StyleSheetListInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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
                return Engine.Select(RefStyleSheetList[index]);
            return base.Get(propertyName);
        }


        public IStyleSheetList RefStyleSheetList
        {
            get;
            set;
        }
    }
}