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
        public HashChangeEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static HashChangeEventInstance CreateHashChangeEventObject(Engine engine)
        {
            var obj = new HashChangeEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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