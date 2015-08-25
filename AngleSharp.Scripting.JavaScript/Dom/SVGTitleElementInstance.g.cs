namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGTitleElementInstance : SVGElementInstance
    {
        readonly EngineInstance _engine;

        public SVGTitleElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGTitleElementInstance CreateSVGTitleElementObject(EngineInstance engine)
        {
            var obj = new SVGTitleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGTitleElement"; }
        }

        public ISvgTitleElement RefSVGTitleElement
        {
            get;
            set;
        }
    }
}