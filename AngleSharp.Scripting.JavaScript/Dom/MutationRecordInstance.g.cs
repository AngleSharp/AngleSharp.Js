namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class MutationRecordInstance : ObjectInstance
    {
        readonly EngineInstance _engine;

        public MutationRecordInstance(EngineInstance engine)
            : base(engine.Jint)
        {
            _engine = engine;
        }

        public static MutationRecordInstance CreateMutationRecordObject(EngineInstance engine)
        {
            var obj = new MutationRecordInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "MutationRecord"; }
        }

        public IMutationRecord RefMutationRecord
        {
            get;
            set;
        }
    }
}