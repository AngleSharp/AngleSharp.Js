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
        readonly EngineInstance _engine;

        public UIEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static UIEventInstance CreateUIEventObject(EngineInstance engine)
        {
            var obj = new UIEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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