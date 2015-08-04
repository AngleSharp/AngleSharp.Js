namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class SVGDocumentInstance : DocumentInstance
    {
        public SVGDocumentInstance(Engine engine)
            : base(engine)
        {
        }

        public static SVGDocumentInstance CreateSVGDocumentObject(Engine engine)
        {
            var obj = new SVGDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "SVGDocument"; }
        }

        public ISvgDocument RefSVGDocument
        {
            get;
            set;
        }
    }
}