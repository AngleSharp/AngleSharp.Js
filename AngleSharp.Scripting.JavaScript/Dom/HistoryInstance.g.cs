namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class HistoryInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public HistoryInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static HistoryInstance CreateHistoryObject(EngineInstance engine)
        {
            var obj = new HistoryInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "History"; }
        }

        public IHistory RefHistory
        {
            get;
            set;
        }
    }
}