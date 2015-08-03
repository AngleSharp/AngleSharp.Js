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