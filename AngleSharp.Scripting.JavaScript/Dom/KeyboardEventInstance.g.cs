namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class KeyboardEventInstance : UIEventInstance
    {
        readonly EngineInstance _engine;

        public KeyboardEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("DOM_KEY_LOCATION_STANDARD", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Standard), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_LEFT", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Left), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_RIGHT", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Right), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_NUMPAD", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.NumPad), false, true, false);
        }

        public static KeyboardEventInstance CreateKeyboardEventObject(EngineInstance engine)
        {
            var obj = new KeyboardEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "KeyboardEvent"; }
        }

        public KeyboardEvent RefKeyboardEvent
        {
            get;
            set;
        }
    }
}