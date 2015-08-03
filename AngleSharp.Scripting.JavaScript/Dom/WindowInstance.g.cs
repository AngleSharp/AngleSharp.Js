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
        public WindowInstance(Engine engine)
            : base(engine)
        {
        }

        public static WindowInstance CreateWindowObject(Engine engine)
        {
            var obj = new WindowInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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