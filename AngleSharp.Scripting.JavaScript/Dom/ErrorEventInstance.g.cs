namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class ErrorEventInstance : EventInstance
    {
        public ErrorEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static ErrorEventInstance CreateErrorEventObject(Engine engine)
        {
            var obj = new ErrorEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "ErrorEvent"; }
        }

        public ErrorEvent RefErrorEvent
        {
            get;
            set;
        }
    }
}