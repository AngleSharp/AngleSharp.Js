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
        readonly EngineInstance _engine;

        public MutationObserverInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static MutationObserverInstance CreateMutationObserverObject(EngineInstance engine)
        {
            var obj = new MutationObserverInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
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