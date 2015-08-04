namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class FocusEventInstance : UIEventInstance
    {
        public FocusEventInstance(Engine engine)
            : base(engine)
        {
        }

        public static FocusEventInstance CreateFocusEventObject(Engine engine)
        {
            var obj = new FocusEventInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "FocusEvent"; }
        }

        public FocusEvent RefFocusEvent
        {
            get;
            set;
        }
    }
}