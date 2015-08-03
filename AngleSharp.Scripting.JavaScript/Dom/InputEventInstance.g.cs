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
        public InputEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static InputEventInstance CreateInputEventObject(Engine engine)
        {
            var obj = new InputEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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