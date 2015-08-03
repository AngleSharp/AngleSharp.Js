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
        public SVGSVGElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGSVGElementInstance CreateSVGSVGElementObject(Engine engine)
        {
            var obj = new SVGSVGElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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