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

    sealed partial class MutationRecordPrototype : MutationRecordInstance
    {
        readonly EngineInstance _engine;

        public MutationRecordPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("target", Engine.AsProperty(GetTarget));
            FastSetProperty("addedNodes", Engine.AsProperty(GetAddedNodes));
            FastSetProperty("removedNodes", Engine.AsProperty(GetRemovedNodes));
            FastSetProperty("previousSibling", Engine.AsProperty(GetPreviousSibling));
            FastSetProperty("nextSibling", Engine.AsProperty(GetNextSibling));
            FastSetProperty("attributeName", Engine.AsProperty(GetAttributeName));
            FastSetProperty("attributeNamespace", Engine.AsProperty(GetAttributeNamespace));
            FastSetProperty("oldValue", Engine.AsProperty(GetOldValue));
        }

        public static MutationRecordPrototype CreatePrototypeObject(EngineInstance engine, MutationRecordConstructor constructor)
        {
            var obj = new MutationRecordPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.Target);
        }


        JsValue GetAddedNodes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.Added);
        }


        JsValue GetRemovedNodes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.Removed);
        }


        JsValue GetPreviousSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.PreviousSibling);
        }


        JsValue GetNextSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.NextSibling);
        }


        JsValue GetAttributeName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.AttributeName);
        }


        JsValue GetAttributeNamespace(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.AttributeNamespace);
        }


        JsValue GetOldValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MutationRecordInstance>(Fail).RefMutationRecord;
            return _engine.GetDomNode(reference.PreviousValue);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MutationRecord]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}