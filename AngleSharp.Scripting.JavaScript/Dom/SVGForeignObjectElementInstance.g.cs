namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGForeignObjectElementInstance : SVGElementInstance
    {
        readonly EngineInstance _engine;

        public SVGForeignObjectElementInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGForeignObjectElementInstance CreateSVGForeignObjectElementObject(EngineInstance engine)
        {
            var obj = new SVGForeignObjectElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGForeignObjectElement"; }
        }

        public ISvgForeignObjectElement RefSVGForeignObjectElement
        {
            get;
            set;
        }
    }
}