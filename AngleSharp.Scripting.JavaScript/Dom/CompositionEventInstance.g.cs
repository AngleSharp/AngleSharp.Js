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
        public CompositionEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static CompositionEventInstance CreateCompositionEventObject(Engine engine)
        {
            var obj = new CompositionEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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