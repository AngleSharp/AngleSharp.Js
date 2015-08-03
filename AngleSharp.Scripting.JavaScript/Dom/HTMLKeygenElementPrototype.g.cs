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

    sealed partial class HTMLKeygenElementPrototype : HTMLKeygenElementInstance
    {
        public HTMLKeygenElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("autofocus", Engine.AsProperty(GetAutofocus, SetAutofocus));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("keytype", Engine.AsProperty(GetKeytype, SetKeytype));
            FastSetProperty("challenge", Engine.AsProperty(GetChallenge, SetChallenge));
        }

        public static HTMLKeygenElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLKeygenElementConstructor constructor)
        {
            var obj = new HTMLKeygenElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetAutofocus(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Autofocus);
        }

        void SetAutofocus(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.Autofocus = value;
        }

        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Labels);
        }


        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Form);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Type);
        }


        JsValue GetKeytype(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.KeyEncryption);
        }

        void SetKeytype(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            var value = TypeConverter.ToString(argument);
            reference.KeyEncryption = value;
        }

        JsValue GetChallenge(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            return Engine.Select(reference.Challenge);
        }

        void SetChallenge(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLKeygenElementInstance>(Fail).RefHTMLKeygenElement;
            var value = TypeConverter.ToString(argument);
            reference.Challenge = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLKeygenElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}