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

    sealed partial class DocumentTypePrototype : DocumentTypeInstance
    {
        public DocumentTypePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName));
            FastSetProperty("publicId", Engine.AsProperty(GetPublicId));
            FastSetProperty("systemId", Engine.AsProperty(GetSystemId));
        }

        public static DocumentTypePrototype CreatePrototypeObject(EngineInstance engine, DocumentTypeConstructor constructor)
        {
            var obj = new DocumentTypePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Node.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            return Engine.Select(reference.Name);
        }


        JsValue GetPublicId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            return Engine.Select(reference.PublicIdentifier);
        }


        JsValue GetSystemId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentTypeInstance>(Fail).RefDocumentType;
            return Engine.Select(reference.SystemIdentifier);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DocumentType]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}