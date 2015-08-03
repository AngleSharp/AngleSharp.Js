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
        public RenderingContextInstance(Engine engine)
            : base(engine)
        {
        }

        public static RenderingContextInstance CreateRenderingContextObject(Engine engine)
        {
            var obj = new RenderingContextInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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