namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGTitleElementInstance : SVGElementInstance
    {
        public SVGTitleElementInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGTitleElementInstance CreateSVGTitleElementObject(Engine engine)
        {
            var obj = new SVGTitleElementInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGTitleElement"; }
        }

        public ISvgTitleElement RefSVGTitleElement
        {
            get;
            set;
        }
    }
}