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

    sealed partial class NodePrototype : NodeInstance
    {
        readonly EngineInstance _engine;

        public NodePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("cloneNode", Engine.AsValue(CloneNode), true, true, true);
            FastAddProperty("isEqualNode", Engine.AsValue(IsEqualNode), true, true, true);
            FastAddProperty("compareDocumentPosition", Engine.AsValue(CompareDocumentPosition), true, true, true);
            FastAddProperty("normalize", Engine.AsValue(Normalize), true, true, true);
            FastAddProperty("contains", Engine.AsValue(Contains), true, true, true);
            FastAddProperty("isDefaultNamespace", Engine.AsValue(IsDefaultNamespace), true, true, true);
            FastAddProperty("lookupNamespaceURI", Engine.AsValue(LookupNamespaceURI), true, true, true);
            FastAddProperty("lookupPrefix", Engine.AsValue(LookupPrefix), true, true, true);
            FastAddProperty("appendChild", Engine.AsValue(AppendChild), true, true, true);
            FastAddProperty("insertBefore", Engine.AsValue(InsertBefore), true, true, true);
            FastAddProperty("removeChild", Engine.AsValue(RemoveChild), true, true, true);
            FastAddProperty("replaceChild", Engine.AsValue(ReplaceChild), true, true, true);
            FastSetProperty("baseURI", Engine.AsProperty(GetBaseURI));
            FastSetProperty("nodeName", Engine.AsProperty(GetNodeName));
            FastSetProperty("childNodes", Engine.AsProperty(GetChildNodes));
            FastSetProperty("ownerDocument", Engine.AsProperty(GetOwnerDocument));
            FastSetProperty("parentElement", Engine.AsProperty(GetParentElement));
            FastSetProperty("parentNode", Engine.AsProperty(GetParentNode));
            FastSetProperty("firstChild", Engine.AsProperty(GetFirstChild));
            FastSetProperty("lastChild", Engine.AsProperty(GetLastChild));
            FastSetProperty("nextSibling", Engine.AsProperty(GetNextSibling));
            FastSetProperty("previousSibling", Engine.AsProperty(GetPreviousSibling));
            FastSetProperty("nodeType", Engine.AsProperty(GetNodeType));
            FastSetProperty("nodeValue", Engine.AsProperty(GetNodeValue, SetNodeValue));
            FastSetProperty("textContent", Engine.AsProperty(GetTextContent, SetTextContent));
            FastSetProperty("hasChildNodes", Engine.AsProperty(GetHasChildNodes));
        }

        public static NodePrototype CreatePrototypeObject(EngineInstance engine, NodeConstructor constructor)
        {
            var obj = new NodePrototype(engine)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue CloneNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var deep = TypeConverter.ToBoolean(arguments.At(0));
            return _engine.GetDomNode(reference.Clone(deep));
        }

        JsValue IsEqualNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var otherNode = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.Equals(otherNode));
        }

        JsValue CompareDocumentPosition(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var otherNode = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.CompareDocumentPosition(otherNode));
        }

        JsValue Normalize(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            reference.Normalize();
            return JsValue.Undefined;
        }

        JsValue Contains(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var otherNode = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.Contains(otherNode));
        }

        JsValue IsDefaultNamespace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.IsDefaultNamespace(namespaceUri));
        }

        JsValue LookupNamespaceURI(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var prefix = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.LookupNamespaceUri(prefix));
        }

        JsValue LookupPrefix(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.LookupPrefix(namespaceUri));
        }

        JsValue AppendChild(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var child = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.AppendChild(child));
        }

        JsValue InsertBefore(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var newElement = DomTypeConverter.ToNode(arguments.At(0));
            var referenceElement = DomTypeConverter.ToNode(arguments.At(1));
            return _engine.GetDomNode(reference.InsertBefore(newElement, referenceElement));
        }

        JsValue RemoveChild(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var child = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.RemoveChild(child));
        }

        JsValue ReplaceChild(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var newChild = DomTypeConverter.ToNode(arguments.At(0));
            var oldChild = DomTypeConverter.ToNode(arguments.At(1));
            return _engine.GetDomNode(reference.ReplaceChild(newChild, oldChild));
        }

        JsValue GetBaseURI(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.BaseUri);
        }


        JsValue GetNodeName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.NodeName);
        }


        JsValue GetChildNodes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.ChildNodes);
        }


        JsValue GetOwnerDocument(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.Owner);
        }


        JsValue GetParentElement(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.ParentElement);
        }


        JsValue GetParentNode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.Parent);
        }


        JsValue GetFirstChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.FirstChild);
        }


        JsValue GetLastChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.LastChild);
        }


        JsValue GetNextSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.NextSibling);
        }


        JsValue GetPreviousSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.PreviousSibling);
        }


        JsValue GetNodeType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.NodeType);
        }


        JsValue GetNodeValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.NodeValue);
        }

        void SetNodeValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var value = TypeConverter.ToString(argument);
            reference.NodeValue = value;
        }

        JsValue GetTextContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.TextContent);
        }

        void SetTextContent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            var value = TypeConverter.ToString(argument);
            reference.TextContent = value;
        }

        JsValue GetHasChildNodes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NodeInstance>(Fail).RefNode;
            return _engine.GetDomNode(reference.HasChildNodes);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Node]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}