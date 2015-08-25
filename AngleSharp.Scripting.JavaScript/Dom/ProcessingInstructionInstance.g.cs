namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime;
    using Jint.Native;
    using Jint.Native.Object;
    using System;

    partial class ProcessingInstructionInstance : CharacterDataInstance
    {
        readonly EngineInstance _engine;

        public ProcessingInstructionInstance(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
        }

        public static ProcessingInstructionInstance CreateProcessingInstructionObject(EngineInstance engine)
        {
            var obj = new ProcessingInstructionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Object.PrototypeObject;            
            return obj;
        }

        public override String Class
        {
            get { return "ProcessingInstruction"; }
        }

        public IProcessingInstruction RefProcessingInstruction
        {
            get;
            set;
        }
    }
}