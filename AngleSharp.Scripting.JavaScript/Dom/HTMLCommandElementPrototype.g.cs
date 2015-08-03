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

    sealed partial class HTMLCommandElementPrototype : HTMLCommandElementInstance
    {
        public HTMLCommandElementPrototype(Engine engine)
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
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
            FastSetProperty("icon", Engine.AsProperty(GetIcon, SetIcon));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("checked", Engine.AsProperty(GetChecked, SetChecked));
            FastSetProperty("radiogroup", Engine.AsProperty(GetRadiogroup, SetRadiogroup));
            FastSetProperty("command", Engine.AsProperty(GetCommand));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLCommandElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLCommandElementConstructor constructor)
        {
            var obj = new HTMLCommandElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue GetIcon(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Icon);
        }

        void SetIcon(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToString(argument);
            reference.Icon = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetChecked(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.IsChecked);
        }

        void SetChecked(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsChecked = value;
        }

        JsValue GetRadiogroup(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.RadioGroup);
        }

        void SetRadiogroup(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToString(argument);
            reference.RadioGroup = value;
        }

        JsValue GetCommand(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Command);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCommandElementInstance>(Fail).RefHTMLCommandElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLCommandElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}