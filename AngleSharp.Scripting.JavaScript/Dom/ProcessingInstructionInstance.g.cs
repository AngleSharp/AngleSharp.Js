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
        public ProcessingInstructionInstance(Engine engine)
            : base(engine)
        {
        }

        public static ProcessingInstructionInstance CreateProcessingInstructionObject(Engine engine)
        {
            var obj = new ProcessingInstructionInstance(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Object.PrototypeObject;            
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