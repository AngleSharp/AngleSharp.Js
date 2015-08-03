namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class DocumentConstructor : FunctionInstance, IConstructor
    {
        public DocumentConstructor(Engine engine)
            : base(engine, null, null, false)
        {
            FastAddProperty("DOCUMENT_POSITION_DISCONNECTED", (UInt32)(AngleSharp.Dom.DocumentPositions.Disconnected), false, true, false);
            FastAddProperty("DOCUMENT_POSITION_PRECEDING", (UInt32)(AngleSharp.Dom.DocumentPositions.Preceding), false, true, false);
            FastAddProperty("DOCUMENT_POSITION_FOLLOWING", (UInt32)(AngleSharp.Dom.DocumentPositions.Following), false, true, false);
            FastAddProperty("DOCUMENT_POSITION_CONTAINS", (UInt32)(AngleSharp.Dom.DocumentPositions.Contains), false, true, false);
            FastAddProperty("DOCUMENT_POSITION_CONTAINED_BY", (UInt32)(AngleSharp.Dom.DocumentPositions.ContainedBy), false, true, false);
            FastAddProperty("DOCUMENT_POSITION_IMPLEMENTATION_SPECIFIC", (UInt32)(AngleSharp.Dom.DocumentPositions.ImplementationSpecific), false, true, false);
            FastAddProperty("ELEMENT_NODE", (UInt32)(AngleSharp.Dom.NodeType.Element), false, true, false);
            FastAddProperty("ATTRIBUTE_NODE", (UInt32)(AngleSharp.Dom.NodeType.Attribute), false, true, false);
            FastAddProperty("TEXT_NODE", (UInt32)(AngleSharp.Dom.NodeType.Text), false, true, false);
            FastAddProperty("CDATA_SECTION_NODE", (UInt32)(AngleSharp.Dom.NodeType.CharacterData), false, true, false);
            FastAddProperty("ENTITY_REFERENCE_NODE", (UInt32)(AngleSharp.Dom.NodeType.EntityReference), false, true, false);
            FastAddProperty("ENTITY_NODE", (UInt32)(AngleSharp.Dom.NodeType.Entity), false, true, false);
            FastAddProperty("PROCESSING_INSTRUCTION_NODE", (UInt32)(AngleSharp.Dom.NodeType.ProcessingInstruction), false, true, false);
            FastAddProperty("COMMENT_NODE", (UInt32)(AngleSharp.Dom.NodeType.Comment), false, true, false);
            FastAddProperty("DOCUMENT_NODE", (UInt32)(AngleSharp.Dom.NodeType.Document), false, true, false);
            FastAddProperty("DOCUMENT_TYPE_NODE", (UInt32)(AngleSharp.Dom.NodeType.DocumentType), false, true, false);
            FastAddProperty("DOCUMENT_FRAGMENT_NODE", (UInt32)(AngleSharp.Dom.NodeType.DocumentFragment), false, true, false);
            FastAddProperty("NOTATION_NODE", (UInt32)(AngleSharp.Dom.NodeType.Notation), false, true, false);
        }

        public DocumentPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static DocumentConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new DocumentConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = DocumentPrototype.CreatePrototypeObject(engine, obj);
            obj.FastAddProperty("length", 0, false, false, false);
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);
            return obj;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}