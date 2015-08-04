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

    sealed partial class HTMLOptionElementPrototype : HTMLOptionElementInstance
    {
        public HTMLOptionElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
            FastSetProperty("defaultSelected", Engine.AsProperty(GetDefaultSelected, SetDefaultSelected));
            FastSetProperty("selected", Engine.AsProperty(GetSelected, SetSelected));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("text", Engine.AsProperty(GetText, SetText));
            FastSetProperty("index", Engine.AsProperty(GetIndex));
        }

        public static HTMLOptionElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLOptionElementConstructor constructor)
        {
            var obj = new HTMLOptionElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.Form);
        }


        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue GetDefaultSelected(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.IsDefaultSelected);
        }

        void SetDefaultSelected(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDefaultSelected = value;
        }

        JsValue GetSelected(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.IsSelected);
        }

        void SetSelected(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsSelected = value;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue GetText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.Text);
        }

        void SetText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            var value = TypeConverter.ToString(argument);
            reference.Text = value;
        }

        JsValue GetIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionElementInstance>(Fail).RefHTMLOptionElement;
            return Engine.Select(reference.Index);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLOptionElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}