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
        readonly EngineInstance _engine;

        public HTMLSelectElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
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
            var obj = new HTMLSelectElementPrototype(engine)
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
            return _engine.GetDomNode(reference.Autofocus);
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
            return _engine.GetDomNode(reference.IsDisabled);
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
            return _engine.GetDomNode(reference.Form);
        }


        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.Labels);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.Name);
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
            return _engine.GetDomNode(reference.Value);
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
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetRequired(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.IsRequired);
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
            return _engine.GetDomNode(reference.SelectedOptions);
        }


        JsValue GetSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.Size);
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
            return _engine.GetDomNode(reference.Options);
        }


        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue GetMultiple(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSelectElementInstance>(Fail).RefHTMLSelectElement;
            return _engine.GetDomNode(reference.IsMultiple);
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
            return _engine.GetDomNode(reference.SelectedIndex);
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