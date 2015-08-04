namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class StyleSheetInstance : ObjectInstance
    {
        public StyleSheetInstance(Engine engine)
            : base(engine)
        {
        }

        public static StyleSheetInstance CreateStyleSheetObject(Engine engine)
        {
            var obj = new StyleSheetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "StyleSheet"; }
        }

        public IStyleSheet RefStyleSheet
        {
            get;
            set;
        }
    }
}