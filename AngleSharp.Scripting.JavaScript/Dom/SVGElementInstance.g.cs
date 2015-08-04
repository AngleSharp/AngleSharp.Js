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
        public SVGElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGElementInstance CreateSVGElementObject(Engine engine)
        {
            var obj = new SVGElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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