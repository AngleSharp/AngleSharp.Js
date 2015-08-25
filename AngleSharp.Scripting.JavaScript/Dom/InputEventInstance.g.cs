namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class InputEventInstance : EventInstance
    {
        readonly EngineInstance _engine;

        public InputEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static InputEventInstance CreateInputEventObject(EngineInstance engine)
        {
            var obj = new InputEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "InputEvent"; }
        }

        public InputEvent RefInputEvent
        {
            get;
            set;
        }
    }
}