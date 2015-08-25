namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGSVGElementInstance : SVGElementInstance
    {
        readonly EngineInstance _engine;

        public SVGSVGElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGSVGElementInstance CreateSVGSVGElementObject(EngineInstance engine)
        {
            var obj = new SVGSVGElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGSVGElement"; }
        }

        public ISvgSvgElement RefSVGSVGElement
        {
            get;
            set;
        }
    }
}