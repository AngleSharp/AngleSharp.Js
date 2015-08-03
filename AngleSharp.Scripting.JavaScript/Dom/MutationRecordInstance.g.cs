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
        public MutationRecordInstance(Engine engine)
            : base(engine)
        {
        }

        public static MutationRecordInstance CreateMutationRecordObject(Engine engine)
        {
            var obj = new MutationRecordInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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