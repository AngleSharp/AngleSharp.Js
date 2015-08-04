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
        public CustomEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static CustomEventInstance CreateCustomEventObject(Engine engine)
        {
            var obj = new CustomEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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