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
        public SVGCircleElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGCircleElementInstance CreateSVGCircleElementObject(Engine engine)
        {
            var obj = new SVGCircleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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