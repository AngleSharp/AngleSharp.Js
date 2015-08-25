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
        readonly EngineInstance _engine;

        public ErrorEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static ErrorEventInstance CreateErrorEventObject(EngineInstance engine)
        {
            var obj = new ErrorEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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