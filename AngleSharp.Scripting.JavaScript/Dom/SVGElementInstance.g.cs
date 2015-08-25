namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGElementInstance : ElementInstance
    {
        readonly EngineInstance _engine;

        public SVGElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGElementInstance CreateSVGElementObject(EngineInstance engine)
        {
            var obj = new SVGElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGElement"; }
        }

        public ISvgElement RefSVGElement
        {
            get;
            set;
        }
    }
}