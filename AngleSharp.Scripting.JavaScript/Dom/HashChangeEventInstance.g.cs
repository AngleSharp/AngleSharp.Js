namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HashChangeEventInstance : EventInstance
    {
        readonly EngineInstance _engine;

        public HashChangeEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static HashChangeEventInstance CreateHashChangeEventObject(EngineInstance engine)
        {
            var obj = new HashChangeEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "HashChangeEvent"; }
        }

        public HashChangedEvent RefHashChangeEvent
        {
            get;
            set;
        }
    }
}