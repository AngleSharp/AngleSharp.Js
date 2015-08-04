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
        public CanvasRenderingContext2DInstance(Engine engine)
            : base(engine)
        {
        }

        public static CanvasRenderingContext2DInstance CreateCanvasRenderingContext2DObject(Engine engine)
        {
            var obj = new CanvasRenderingContext2DInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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