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

    sealed partial class HTMLButtonElementPrototype : HTMLButtonElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLButtonElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("autofocus", Engine.AsProperty(GetAutofocus, SetAutofocus));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("formAction", Engine.AsProperty(GetFormAction, SetFormAction));
            FastSetProperty("formEncType", Engine.AsProperty(GetFormEncType, SetFormEncType));
            FastSetProperty("formMethod", Engine.AsProperty(GetFormMethod, SetFormMethod));
            FastSetProperty("formNoValidate", Engine.AsProperty(GetFormNoValidate, SetFormNoValidate));
            FastSetProperty("formTarget", Engine.AsProperty(GetFormTarget, SetFormTarget));
        }

        public static HTMLButtonElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLButtonElementConstructor constructor)
        {
            var obj = new HTMLButtonElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetAutofocus(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Autofocus);
        }

        void SetAutofocus(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.Autofocus = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Form);
        }


        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Labels);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue GetFormAction(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.FormAction);
        }

        void SetFormAction(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.FormAction = value;
        }

        JsValue GetFormEncType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.FormEncType);
        }

        void SetFormEncType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.FormEncType = value;
        }

        JsValue GetFormMethod(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.FormMethod);
        }

        void SetFormMethod(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.FormMethod = value;
        }

        JsValue GetFormNoValidate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.FormNoValidate);
        }

        void SetFormNoValidate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.FormNoValidate = value;
        }

        JsValue GetFormTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            return _engine.GetDomNode(reference.FormTarget);
        }

        void SetFormTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLButtonElementInstance>(Fail).RefHTMLButtonElement;
            var value = TypeConverter.ToString(argument);
            reference.FormTarget = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLButtonElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}