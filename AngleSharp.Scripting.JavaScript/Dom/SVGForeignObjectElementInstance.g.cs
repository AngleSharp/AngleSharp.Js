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
        public SVGForeignObjectElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGForeignObjectElementInstance CreateSVGForeignObjectElementObject(Engine engine)
        {
            var obj = new SVGForeignObjectElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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