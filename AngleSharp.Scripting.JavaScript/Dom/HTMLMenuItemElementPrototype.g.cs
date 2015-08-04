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

    sealed partial class HTMLMenuItemElementPrototype : HTMLMenuItemElementInstance
    {
        public HTMLMenuItemElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("command", Engine.AsProperty(GetCommand));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
            FastSetProperty("icon", Engine.AsProperty(GetIcon, SetIcon));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("checked", Engine.AsProperty(GetChecked, SetChecked));
            FastSetProperty("default", Engine.AsProperty(GetDefault, SetDefault));
            FastSetProperty("radiogroup", Engine.AsProperty(GetRadiogroup, SetRadiogroup));
        }

        public static HTMLMenuItemElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMenuItemElementConstructor constructor)
        {
            var obj = new HTMLMenuItemElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCommand(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.Command);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue GetIcon(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.Icon);
        }

        void SetIcon(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToString(argument);
            reference.Icon = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetChecked(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.IsChecked);
        }

        void SetChecked(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsChecked = value;
        }

        JsValue GetDefault(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.IsDefault);
        }

        void SetDefault(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDefault = value;
        }

        JsValue GetRadiogroup(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            return Engine.Select(reference.RadioGroup);
        }

        void SetRadiogroup(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuItemElementInstance>(Fail).RefHTMLMenuItemElement;
            var value = TypeConverter.ToString(argument);
            reference.RadioGroup = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLMenuItemElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}