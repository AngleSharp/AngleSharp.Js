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

    sealed partial class HTMLObjectElementPrototype : HTMLObjectElementInstance
    {
        public HTMLObjectElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("data", Engine.AsProperty(GetData, SetData));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("typeMustMatch", Engine.AsProperty(GetTypeMustMatch, SetTypeMustMatch));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("useMap", Engine.AsProperty(GetUseMap, SetUseMap));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("contentDocument", Engine.AsProperty(GetContentDocument));
            FastSetProperty("contentWindow", Engine.AsProperty(GetContentWindow));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLObjectElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLObjectElementConstructor constructor)
        {
            var obj = new HTMLObjectElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetData(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Source);
        }

        void SetData(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetTypeMustMatch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.TypeMustMatch);
        }

        void SetTypeMustMatch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.TypeMustMatch = value;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetUseMap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.UseMap);
        }

        void SetUseMap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.UseMap = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Form);
        }


        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayHeight = value;
        }

        JsValue GetContentDocument(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.ContentDocument);
        }


        JsValue GetContentWindow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.ContentWindow);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLObjectElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}