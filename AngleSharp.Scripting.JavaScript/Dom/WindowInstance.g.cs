namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class WindowInstance : EventTargetInstance
    {
        readonly EngineInstance _engine;

        public WindowInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static WindowInstance CreateWindowObject(EngineInstance engine)
        {
            var obj = new WindowInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "Window"; }
        }

        public IWindow RefWindow
        {
            get;
            set;
        }
    }
}