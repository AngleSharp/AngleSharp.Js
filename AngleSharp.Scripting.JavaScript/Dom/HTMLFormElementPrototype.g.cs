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

    sealed partial class HTMLFormElementPrototype : HTMLFormElementInstance
    {
        public HTMLFormElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("submit", Engine.AsValue(Submit), true, true, true);
            FastAddProperty("reset", Engine.AsValue(Reset), true, true, true);
            FastAddProperty("checkValidity", Engine.AsValue(CheckValidity), true, true, true);
            FastAddProperty("reportValidity", Engine.AsValue(ReportValidity), true, true, true);
            FastAddProperty("requestAutocomplete", Engine.AsValue(RequestAutocomplete), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("acceptCharset", Engine.AsProperty(GetAcceptCharset, SetAcceptCharset));
            FastSetProperty("action", Engine.AsProperty(GetAction, SetAction));
            FastSetProperty("autocomplete", Engine.AsProperty(GetAutocomplete, SetAutocomplete));
            FastSetProperty("enctype", Engine.AsProperty(GetEnctype, SetEnctype));
            FastSetProperty("encoding", Engine.AsProperty(GetEncoding, SetEncoding));
            FastSetProperty("method", Engine.AsProperty(GetMethod, SetMethod));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("noValidate", Engine.AsProperty(GetNoValidate, SetNoValidate));
            FastSetProperty("target", Engine.AsProperty(GetTarget, SetTarget));
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("elements", Engine.AsProperty(GetElements));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLFormElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLFormElementConstructor constructor)
        {
            var obj = new HTMLFormElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Submit(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Submit());
        }

        JsValue Reset(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            reference.Reset();
            return JsValue.Undefined;
        }

        JsValue CheckValidity(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.CheckValidity());
        }

        JsValue ReportValidity(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.ReportValidity());
        }

        JsValue RequestAutocomplete(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            reference.RequestAutocomplete();
            return JsValue.Undefined;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetAcceptCharset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.AcceptCharset);
        }

        void SetAcceptCharset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.AcceptCharset = value;
        }

        JsValue GetAction(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Action);
        }

        void SetAction(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Action = value;
        }

        JsValue GetAutocomplete(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Autocomplete);
        }

        void SetAutocomplete(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Autocomplete = value;
        }

        JsValue GetEnctype(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Enctype);
        }

        void SetEnctype(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Enctype = value;
        }

        JsValue GetEncoding(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Encoding);
        }

        void SetEncoding(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Encoding = value;
        }

        JsValue GetMethod(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Method);
        }

        void SetMethod(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Method = value;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetNoValidate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.NoValidate);
        }

        void SetNoValidate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.NoValidate = value;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Target);
        }

        void SetTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Target = value;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Length);
        }


        JsValue GetElements(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Elements);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFormElementInstance>(Fail).RefHTMLFormElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLFormElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}