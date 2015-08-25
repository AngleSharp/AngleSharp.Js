namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class CompositionEventInstance : UIEventInstance
    {
        readonly EngineInstance _engine;

        public CompositionEventInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static CompositionEventInstance CreateCompositionEventObject(EngineInstance engine)
        {
            var obj = new CompositionEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "CompositionEvent"; }
        }

        public CompositionEvent RefCompositionEvent
        {
            get;
            set;
        }
    }
}