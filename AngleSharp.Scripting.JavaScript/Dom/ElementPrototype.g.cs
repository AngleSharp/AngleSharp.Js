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

    sealed partial class ElementPrototype : ElementInstance
    {
        public ElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("insertAdjacentHTML", Engine.AsValue(InsertAdjacentHTML), true, true, true);
            FastAddProperty("hasAttribute", Engine.AsValue(HasAttribute), true, true, true);
            FastAddProperty("hasAttributeNS", Engine.AsValue(HasAttributeNS), true, true, true);
            FastAddProperty("getAttribute", Engine.AsValue(GetAttribute), true, true, true);
            FastAddProperty("getAttributeNS", Engine.AsValue(GetAttributeNS), true, true, true);
            FastAddProperty("setAttribute", Engine.AsValue(SetAttribute), true, true, true);
            FastAddProperty("setAttributeNS", Engine.AsValue(SetAttributeNS), true, true, true);
            FastAddProperty("removeAttribute", Engine.AsValue(RemoveAttribute), true, true, true);
            FastAddProperty("removeAttributeNS", Engine.AsValue(RemoveAttributeNS), true, true, true);
            FastAddProperty("getElementsByClassName", Engine.AsValue(GetElementsByClassName), true, true, true);
            FastAddProperty("getElementsByTagName", Engine.AsValue(GetElementsByTagName), true, true, true);
            FastAddProperty("getElementsByTagNameNS", Engine.AsValue(GetElementsByTagNameNS), true, true, true);
            FastAddProperty("matches", Engine.AsValue(Matches), true, true, true);
            FastAddProperty("pseudo", Engine.AsValue(Pseudo), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("prefix", Engine.AsProperty(GetPrefix));
            FastSetProperty("localName", Engine.AsProperty(GetLocalName));
            FastSetProperty("namespaceURI", Engine.AsProperty(GetNamespaceURI));
            FastSetProperty("attributes", Engine.AsProperty(GetAttributes));
            FastSetProperty("classList", Engine.AsProperty(GetClassList));
            FastSetProperty("className", Engine.AsProperty(GetClassName, SetClassName));
            FastSetProperty("id", Engine.AsProperty(GetId, SetId));
            FastSetProperty("innerHTML", Engine.AsProperty(GetInnerHTML, SetInnerHTML));
            FastSetProperty("outerHTML", Engine.AsProperty(GetOuterHTML, SetOuterHTML));
            FastSetProperty("tagName", Engine.AsProperty(GetTagName));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
        }

        public static ElementPrototype CreatePrototypeObject(EngineInstance engine, ElementConstructor constructor)
        {
            var obj = new ElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Node.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InsertAdjacentHTML(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var position = DomTypeConverter.ToAdjacentPosition(arguments.At(0));
            var html = TypeConverter.ToString(arguments.At(1));
            reference.Insert(position, html);
            return JsValue.Undefined;
        }

        JsValue HasAttribute(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.HasAttribute(name));
        }

        JsValue HasAttributeNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var localName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.HasAttribute(namespaceUri, localName));
        }

        JsValue GetAttribute(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetAttribute(name));
        }

        JsValue GetAttributeNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var localName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.GetAttribute(namespaceUri, localName));
        }

        JsValue SetAttribute(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var name = TypeConverter.ToString(arguments.At(0));
            var value = TypeConverter.ToString(arguments.At(1));
            reference.SetAttribute(name, value);
            return JsValue.Undefined;
        }

        JsValue SetAttributeNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var name = TypeConverter.ToString(arguments.At(1));
            var value = TypeConverter.ToString(arguments.At(2));
            reference.SetAttribute(namespaceUri, name, value);
            return JsValue.Undefined;
        }

        JsValue RemoveAttribute(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var name = TypeConverter.ToString(arguments.At(0));
            reference.RemoveAttribute(name);
            return JsValue.Undefined;
        }

        JsValue RemoveAttributeNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var localName = TypeConverter.ToString(arguments.At(1));
            reference.RemoveAttribute(namespaceUri, localName);
            return JsValue.Undefined;
        }

        JsValue GetElementsByClassName(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var classNames = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementsByClassName(classNames));
        }

        JsValue GetElementsByTagName(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var tagName = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementsByTagName(tagName));
        }

        JsValue GetElementsByTagNameNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var tagName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.GetElementsByTagNameNS(namespaceUri, tagName));
        }

        JsValue Matches(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.Matches(selectors));
        }

        JsValue Pseudo(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var pseudoElement = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.Pseudo(pseudoElement));
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetPrefix(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.Prefix);
        }


        JsValue GetLocalName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.LocalName);
        }


        JsValue GetNamespaceURI(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.NamespaceUri);
        }


        JsValue GetAttributes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.Attributes);
        }


        JsValue GetClassList(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.ClassList);
        }


        JsValue GetClassName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.ClassName);
        }

        void SetClassName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var value = TypeConverter.ToString(argument);
            reference.ClassName = value;
        }

        JsValue GetId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.Id);
        }

        void SetId(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var value = TypeConverter.ToString(argument);
            reference.Id = value;
        }

        JsValue GetInnerHTML(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.InnerHtml);
        }

        void SetInnerHTML(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var value = TypeConverter.ToString(argument);
            reference.InnerHtml = value;
        }

        JsValue GetOuterHTML(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.OuterHtml);
        }

        void SetOuterHTML(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            var value = TypeConverter.ToString(argument);
            reference.OuterHtml = value;
        }

        JsValue GetTagName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.TagName);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ElementInstance>(Fail).RefElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Element]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}