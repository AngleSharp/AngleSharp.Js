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

    sealed partial class HTMLSelectElementPrototype : HTMLSelectElementInstance
    {
        public HTMLSelectElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("add", Engine.AsValue(Add), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastSetProperty("autofocus", Engine.AsProperty(GetAutofocus, SetAutofocus));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("required", Engine.AsProperty(GetRequired, SetRequired));
            FastSetProperty("selectedOptions", Engine.AsProperty(GetSelectedOptions));
            FastSetProperty("size", Engine.AsProperty(GetSize, SetSize));
            FastSetProperty("options", Engine.AsProperty(GetOptions));
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("multiple", Engine.AsProperty(GetMultiple, SetMultiple));
            FastSetProperty("selectedIndex", Engine.AsProperty(GetSelectedIndex));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLSelectElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLSelectElementConstructor constructor)
        {
            var obj = new HTMLSelectElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Add(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var element = DomTypeConverter.ToOptionsGroupElement(arguments.At(0));
            var before = DomTypeConverter.ToHtmlElement(arguments.At(1));
            reference.AddOption(element, before);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue GetAutofocus(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Autofocus);
        }

        void SetAutofocus(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.Autofocus = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Form);
        }


        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Labels);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Type);
        }


        JsValue GetRequired(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.IsRequired);
        }

        void SetRequired(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsRequired = value;
        }

        JsValue GetSelectedOptions(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.SelectedOptions);
        }


        JsValue GetSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Size);
        }

        void SetSize(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Size = value;
        }

        JsValue GetOptions(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Options);
        }


        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Length);
        }


        JsValue GetMultiple(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.IsMultiple);
        }

        void SetMultiple(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsMultiple = value;
        }

        JsValue GetSelectedIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.SelectedIndex);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLSelectElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}