namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CustomEventInstance : EventInstance
    {
        readonly EngineInstance _engine;

        public CustomEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CustomEventInstance CreateCustomEventObject(EngineInstance engine)
        {
            var obj = new CustomEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CustomEvent"; }
        }

        public CustomEvent RefCustomEvent
        {
            get;
            set;
        }
    }
}