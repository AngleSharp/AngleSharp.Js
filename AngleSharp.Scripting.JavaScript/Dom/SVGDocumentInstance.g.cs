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
        readonly EngineInstance _engine;

        public SVGDocumentInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static SVGDocumentInstance CreateSVGDocumentObject(EngineInstance engine)
        {
            var obj = new SVGDocumentInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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