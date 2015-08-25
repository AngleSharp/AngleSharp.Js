namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGDescElementInstance : SVGElementInstance
    {
        readonly EngineInstance _engine;

        public SVGDescElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGDescElementInstance CreateSVGDescElementObject(EngineInstance engine)
        {
            var obj = new SVGDescElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGDescElement"; }
        }

        public ISvgDescriptionElement RefSVGDescElement
        {
            get;
            set;
        }
    }
}