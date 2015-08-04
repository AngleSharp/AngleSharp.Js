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
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveOptionAt(index);
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