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

    sealed partial class HTMLInputElementPrototype : HTMLInputElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLInputElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("stepUp", Engine.AsValue(StepUp), true, true, true);
            FastAddProperty("stepDown", Engine.AsValue(StepDown), true, true, true);
            FastAddProperty("select", Engine.AsValue(Select), true, true, true);
            FastAddProperty("setSelectionRange", Engine.AsValue(SetSelectionRange), true, true, true);
            FastSetProperty("autofocus", Engine.AsProperty(GetAutofocus, SetAutofocus));
            FastSetProperty("accept", Engine.AsProperty(GetAccept, SetAccept));
            FastSetProperty("autocomplete", Engine.AsProperty(GetAutocomplete, SetAutocomplete));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
            FastSetProperty("files", Engine.AsProperty(GetFiles));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("required", Engine.AsProperty(GetRequired, SetRequired));
            FastSetProperty("readOnly", Engine.AsProperty(GetReadOnly, SetReadOnly));
            FastSetProperty("alt", Engine.AsProperty(GetAlt, SetAlt));
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("max", Engine.AsProperty(GetMax, SetMax));
            FastSetProperty("min", Engine.AsProperty(GetMin, SetMin));
            FastSetProperty("pattern", Engine.AsProperty(GetPattern, SetPattern));
            FastSetProperty("step", Engine.AsProperty(GetStep, SetStep));
            FastSetProperty("list", Engine.AsProperty(GetList));
            FastSetProperty("formAction", Engine.AsProperty(GetFormAction, SetFormAction));
            FastSetProperty("formEncType", Engine.AsProperty(GetFormEncType, SetFormEncType));
            FastSetProperty("formMethod", Engine.AsProperty(GetFormMethod, SetFormMethod));
            FastSetProperty("formNoValidate", Engine.AsProperty(GetFormNoValidate, SetFormNoValidate));
            FastSetProperty("formTarget", Engine.AsProperty(GetFormTarget, SetFormTarget));
            FastSetProperty("defaultValue", Engine.AsProperty(GetDefaultValue, SetDefaultValue));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("valueAsNumber", Engine.AsProperty(GetValueAsNumber, SetValueAsNumber));
            FastSetProperty("valueAsDate", Engine.AsProperty(GetValueAsDate, SetValueAsDate));
            FastSetProperty("indeterminate", Engine.AsProperty(GetIndeterminate, SetIndeterminate));
            FastSetProperty("defaultChecked", Engine.AsProperty(GetDefaultChecked, SetDefaultChecked));
            FastSetProperty("checked", Engine.AsProperty(GetChecked, SetChecked));
            FastSetProperty("size", Engine.AsProperty(GetSize, SetSize));
            FastSetProperty("multiple", Engine.AsProperty(GetMultiple, SetMultiple));
            FastSetProperty("maxLength", Engine.AsProperty(GetMaxLength, SetMaxLength));
            FastSetProperty("placeholder", Engine.AsProperty(GetPlaceholder, SetPlaceholder));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("selectionDirection", Engine.AsProperty(GetSelectionDirection));
            FastSetProperty("dirName", Engine.AsProperty(GetDirName, SetDirName));
            FastSetProperty("selectionStart", Engine.AsProperty(GetSelectionStart, SetSelectionStart));
            FastSetProperty("selectionEnd", Engine.AsProperty(GetSelectionEnd, SetSelectionEnd));
        }

        public static HTMLInputElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLInputElementConstructor constructor)
        {
            var obj = new HTMLInputElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue StepUp(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var n = TypeConverter.ToInt32(arguments.At(0));
            reference.StepUp(n);
            return JsValue.Undefined;
        }

        JsValue StepDown(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var n = TypeConverter.ToInt32(arguments.At(0));
            reference.StepDown(n);
            return JsValue.Undefined;
        }

        JsValue Select(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            reference.SelectAll();
            return JsValue.Undefined;
        }

        JsValue SetSelectionRange(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var selectionStart = TypeConverter.ToInt32(arguments.At(0));
            var selectionEnd = TypeConverter.ToInt32(arguments.At(1));
            var selectionDirection = TypeConverter.ToString(arguments.At(2));
            reference.Select(selectionStart, selectionEnd, selectionDirection);
            return JsValue.Undefined;
        }

        JsValue GetAutofocus(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Autofocus);
        }

        void SetAutofocus(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.Autofocus = value;
        }

        JsValue GetAccept(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Accept);
        }

        void SetAccept(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Accept = value;
        }

        JsValue GetAutocomplete(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Autocomplete);
        }

        void SetAutocomplete(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Autocomplete = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Form);
        }


        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Labels);
        }


        JsValue GetFiles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Files);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetRequired(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsRequired);
        }

        void SetRequired(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsRequired = value;
        }

        JsValue GetReadOnly(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsReadOnly);
        }

        void SetReadOnly(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsReadOnly = value;
        }

        JsValue GetAlt(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.AlternativeText);
        }

        void SetAlt(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.AlternativeText = value;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetMax(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Maximum);
        }

        void SetMax(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Maximum = value;
        }

        JsValue GetMin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Minimum);
        }

        void SetMin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Minimum = value;
        }

        JsValue GetPattern(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Pattern);
        }

        void SetPattern(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Pattern = value;
        }

        JsValue GetStep(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Step);
        }

        void SetStep(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Step = value;
        }

        JsValue GetList(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.List);
        }


        JsValue GetFormAction(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.FormAction);
        }

        void SetFormAction(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.FormAction = value;
        }

        JsValue GetFormEncType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.FormEncType);
        }

        void SetFormEncType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.FormEncType = value;
        }

        JsValue GetFormMethod(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.FormMethod);
        }

        void SetFormMethod(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.FormMethod = value;
        }

        JsValue GetFormNoValidate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.FormNoValidate);
        }

        void SetFormNoValidate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.FormNoValidate = value;
        }

        JsValue GetFormTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.FormTarget);
        }

        void SetFormTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.FormTarget = value;
        }

        JsValue GetDefaultValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.DefaultValue);
        }

        void SetDefaultValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.DefaultValue = value;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue GetValueAsNumber(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.ValueAsNumber);
        }

        void SetValueAsNumber(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToNumber(argument);
            reference.ValueAsNumber = value;
        }

        JsValue GetValueAsDate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.ValueAsDate);
        }

        void SetValueAsDate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = SystemTypeConverter.ToOptionalDateTime(argument);
            reference.ValueAsDate = value;
        }

        JsValue GetIndeterminate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsIndeterminate);
        }

        void SetIndeterminate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsIndeterminate = value;
        }

        JsValue GetDefaultChecked(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsDefaultChecked);
        }

        void SetDefaultChecked(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDefaultChecked = value;
        }

        JsValue GetChecked(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsChecked);
        }

        void SetChecked(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsChecked = value;
        }

        JsValue GetSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Size);
        }

        void SetSize(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Size = value;
        }

        JsValue GetMultiple(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.IsMultiple);
        }

        void SetMultiple(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsMultiple = value;
        }

        JsValue GetMaxLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.MaxLength);
        }

        void SetMaxLength(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.MaxLength = value;
        }

        JsValue GetPlaceholder(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.Placeholder);
        }

        void SetPlaceholder(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.Placeholder = value;
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayHeight = value;
        }

        JsValue GetSelectionDirection(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.SelectionDirection);
        }


        JsValue GetDirName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.DirectionName);
        }

        void SetDirName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToString(argument);
            reference.DirectionName = value;
        }

        JsValue GetSelectionStart(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.SelectionStart);
        }

        void SetSelectionStart(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.SelectionStart = value;
        }

        JsValue GetSelectionEnd(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            return _engine.GetDomNode(reference.SelectionEnd);
        }

        void SetSelectionEnd(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLInputElementInstance>(Fail).RefHTMLInputElement;
            var value = TypeConverter.ToInt32(argument);
            reference.SelectionEnd = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLInputElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}