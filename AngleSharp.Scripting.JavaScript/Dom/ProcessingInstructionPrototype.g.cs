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
        public ProcessingInstructionPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("target", Engine.AsProperty(GetTarget));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
        }

        public static ProcessingInstructionPrototype CreatePrototypeObject(EngineInstance engine, ProcessingInstructionConstructor constructor)
        {
            var obj = new ProcessingInstructionPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.CharacterData.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            return Engine.Select(reference.Target);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ProcessingInstructionInstance>(Fail).RefProcessingInstruction;
            return Engine.Select(reference.PreviousElementSibling);
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