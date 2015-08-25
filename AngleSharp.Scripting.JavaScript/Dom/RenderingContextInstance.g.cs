namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class RenderingContextInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public RenderingContextInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static RenderingContextInstance CreateRenderingContextObject(EngineInstance engine)
        {
            var obj = new RenderingContextInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "RenderingContext"; }
        }

        public IRenderingContext RefRenderingContext
        {
            get;
            set;
        }
    }
}