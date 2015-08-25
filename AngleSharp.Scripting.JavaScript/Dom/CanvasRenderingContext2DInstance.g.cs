namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CanvasRenderingContext2DInstance : RenderingContextInstance
    {
        readonly EngineInstance _engine;

        public CanvasRenderingContext2DInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CanvasRenderingContext2DInstance CreateCanvasRenderingContext2DObject(EngineInstance engine)
        {
            var obj = new CanvasRenderingContext2DInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CanvasRenderingContext2D"; }
        }

        public ICanvasRenderingContext2D RefCanvasRenderingContext2D
        {
            get;
            set;
        }
    }
}