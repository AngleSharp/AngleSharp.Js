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

    sealed partial class DocumentFragmentPrototype : DocumentFragmentInstance
    {
        public DocumentFragmentPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("getElementById", Engine.AsValue(GetElementById), true, true, true);
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
        }

        public static DocumentFragmentPrototype CreatePrototypeObject(EngineInstance engine, DocumentFragmentConstructor constructor)
        {
            var obj = new DocumentFragmentPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Node.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue GetElementById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            var elementId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementById(elementId));
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentFragmentInstance>(Fail).RefDocumentFragment;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DocumentFragment]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}