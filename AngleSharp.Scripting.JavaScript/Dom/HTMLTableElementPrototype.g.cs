namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class HTMLTableElementPrototype : HTMLTableElementInstance
    {
        public HTMLTableElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("createCaption", Engine.AsValue(CreateCaption), true, true, true);
            FastAddProperty("deleteCaption", Engine.AsValue(DeleteCaption), true, true, true);
            FastAddProperty("createTHead", Engine.AsValue(CreateTHead), true, true, true);
            FastAddProperty("deleteTHead", Engine.AsValue(DeleteTHead), true, true, true);
            FastAddProperty("createTFoot", Engine.AsValue(CreateTFoot), true, true, true);
            FastAddProperty("deleteTFoot", Engine.AsValue(DeleteTFoot), true, true, true);
            FastAddProperty("createTBody", Engine.AsValue(CreateTBody), true, true, true);
            FastAddProperty("insertRow", Engine.AsValue(InsertRow), true, true, true);
            FastAddProperty("deleteRow", Engine.AsValue(DeleteRow), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("caption", Engine.AsProperty(GetCaption, SetCaption));
            FastSetProperty("tHead", Engine.AsProperty(GetTHead, SetTHead));
            FastSetProperty("tFoot", Engine.AsProperty(GetTFoot, SetTFoot));
            FastSetProperty("tBodies", Engine.AsProperty(GetTBodies));
            FastSetProperty("rows", Engine.AsProperty(GetRows));
            FastSetProperty("border", Engine.AsProperty(GetBorder, SetBorder));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLTableElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableElementConstructor constructor)
        {
            var obj = new HTMLTableElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue CreateCaption(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.CreateCaption());
        }

        JsValue DeleteCaption(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteCaption();
            return JsValue.Undefined;
        }

        JsValue CreateTHead(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.CreateHead());
        }

        JsValue DeleteTHead(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteHead();
            return JsValue.Undefined;
        }

        JsValue CreateTFoot(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.CreateFoot());
        }

        JsValue DeleteTFoot(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteFoot();
            return JsValue.Undefined;
        }

        JsValue CreateTBody(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.CreateBody());
        }

        JsValue InsertRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return Engine.Select(reference.InsertRowAt(index));
        }

        JsValue DeleteRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveRowAt(index);
            return JsValue.Undefined;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetCaption(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Caption);
        }

        void SetCaption(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableCaptionElement(argument);
            reference.Caption = value;
        }

        JsValue GetTHead(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Head);
        }

        void SetTHead(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableSectionElement(argument);
            reference.Head = value;
        }

        JsValue GetTFoot(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Foot);
        }

        void SetTFoot(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableSectionElement(argument);
            reference.Foot = value;
        }

        JsValue GetTBodies(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Bodies);
        }


        JsValue GetRows(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Rows);
        }


        JsValue GetBorder(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Border);
        }

        void SetBorder(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = TypeConverter.ToUint32(argument);
            reference.Border = value;
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}