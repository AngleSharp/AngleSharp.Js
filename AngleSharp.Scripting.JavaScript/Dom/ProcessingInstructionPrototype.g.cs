namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class ProcessingInstructionPrototype : ProcessingInstructionInstance
    {
        readonly EngineInstance _engine;

        public ProcessingInstructionPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("target", Engine.AsProperty(GetTarget));
        }

        public static ProcessingInstructionPrototype CreatePrototypeObject(EngineInstance engine, ProcessingInstructionConstructor constructor)
        {
            var obj = new ProcessingInstructionPrototype(engine)
            {
                Prototype = engine.Constructors.CharacterData.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            return _engine.GetDomNode(reference.Target);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object ProcessingInstruction]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}