namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class UIEventInstance : EventInstance
    {
        public UIEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static UIEventInstance CreateUIEventObject(Engine engine)
        {
            var obj = new UIEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "UIEvent"; }
        }

        public UiEvent RefUIEvent
        {
            get;
            set;
        }
    }
}