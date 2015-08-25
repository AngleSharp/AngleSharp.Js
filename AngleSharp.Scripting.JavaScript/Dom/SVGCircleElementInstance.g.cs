namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGCircleElementInstance : SVGElementInstance
    {
        readonly EngineInstance _engine;

        public SVGCircleElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGCircleElementInstance CreateSVGCircleElementObject(EngineInstance engine)
        {
            var obj = new SVGCircleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGCircleElement"; }
        }

        public ISvgCircleElement RefSVGCircleElement
        {
            get;
            set;
        }
    }
}