namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MutationObserverInstance : ObjectInstance
    {
        public MutationObserverInstance(Engine engine)
            : base(engine)
        {
        }

        public static MutationObserverInstance CreateMutationObserverObject(Engine engine)
        {
            var obj = new MutationObserverInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MutationObserver"; }
        }

        public MutationObserver RefMutationObserver
        {
            get;
            set;
        }
    }
}