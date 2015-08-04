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

    sealed partial class NodeIteratorPrototype : NodeIteratorInstance
    {
        public NodeIteratorPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("nextNode", Engine.AsValue(NextNode), true, true, true);
            FastAddProperty("previousNode", Engine.AsValue(PreviousNode), true, true, true);
            FastSetProperty("root", Engine.AsProperty(GetRoot));
            FastSetProperty("referenceNode", Engine.AsProperty(GetReferenceNode));
            FastSetProperty("pointerBeforeReferenceNode", Engine.AsProperty(GetPointerBeforeReferenceNode));
            FastSetProperty("whatToShow", Engine.AsProperty(GetWhatToShow));
            FastSetProperty("filter", Engine.AsProperty(GetFilter));
        }

        public static NodeIteratorPrototype CreatePrototypeObject(EngineInstance engine, NodeIteratorConstructor constructor)
        {
            var obj = new NodeIteratorPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue NextNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Next());
        }

        JsValue PreviousNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Previous());
        }

        JsValue GetRoot(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Root);
        }


        JsValue GetReferenceNode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Reference);
        }


        JsValue GetPointerBeforeReferenceNode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.IsBeforeReference);
        }


        JsValue GetWhatToShow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Settings);
        }


        JsValue GetFilter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeIteratorInstance>(Fail).RefNodeIterator;
            return Engine.Select(reference.Filter);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object NodeIterator]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}