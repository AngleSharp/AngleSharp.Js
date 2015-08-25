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
        readonly EngineInstance _engine;

        public StyleSheetInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static StyleSheetInstance CreateStyleSheetObject(EngineInstance engine)
        {
            var obj = new StyleSheetInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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