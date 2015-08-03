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
        public SVGDescElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGDescElementInstance CreateSVGDescElementObject(Engine engine)
        {
            var obj = new SVGDescElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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