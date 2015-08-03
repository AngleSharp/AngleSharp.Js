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

    sealed partial class HTMLLabelElementPrototype : HTMLLabelElementInstance
    {
        public HTMLLabelElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("htmlFor", Engine.AsProperty(GetHtmlFor, SetHtmlFor));
            FastSetProperty("control", Engine.AsProperty(GetControl));
        }

        public static HTMLLabelElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLLabelElementConstructor constructor)
        {
            var obj = new HTMLLabelElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLabelElementInstance>(Fail).RefHTMLLabelElement;
            return Engine.Select(reference.Form);
        }


        JsValue GetHtmlFor(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLabelElementInstance>(Fail).RefHTMLLabelElement;
            return Engine.Select(reference.HtmlFor);
        }

        void SetHtmlFor(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLabelElementInstance>(Fail).RefHTMLLabelElement;
            var value = TypeConverter.ToString(argument);
            reference.HtmlFor = value;
        }

        JsValue GetControl(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLabelElementInstance>(Fail).RefHTMLLabelElement;
            return Engine.Select(reference.Control);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLLabelElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}