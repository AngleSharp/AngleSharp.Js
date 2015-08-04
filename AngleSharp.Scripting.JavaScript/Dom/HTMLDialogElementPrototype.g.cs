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

    sealed partial class HTMLDialogElementPrototype : HTMLDialogElementInstance
    {
        public HTMLDialogElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("show", Engine.AsValue(Show), true, true, true);
            FastAddProperty("showModal", Engine.AsValue(ShowModal), true, true, true);
            FastAddProperty("close", Engine.AsValue(Close), true, true, true);
            FastSetProperty("open", Engine.AsProperty(GetOpen, SetOpen));
            FastSetProperty("returnValue", Engine.AsProperty(GetReturnValue, SetReturnValue));
        }

        public static HTMLDialogElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLDialogElementConstructor constructor)
        {
            var obj = new HTMLDialogElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Show(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            var anchor = DomTypeConverter.ToElement(arguments.At(0));
            reference.Show(anchor);
            return JsValue.Undefined;
        }

        JsValue ShowModal(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            var anchor = DomTypeConverter.ToElement(arguments.At(0));
            reference.ShowModal(anchor);
            return JsValue.Undefined;
        }

        JsValue Close(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            var returnValue = TypeConverter.ToString(arguments.At(0));
            reference.Close(returnValue);
            return JsValue.Undefined;
        }

        JsValue GetOpen(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            return Engine.Select(reference.Open);
        }

        void SetOpen(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.Open = value;
        }

        JsValue GetReturnValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            return Engine.Select(reference.ReturnValue);
        }

        void SetReturnValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLDialogElementInstance>(Fail).RefHTMLDialogElement;
            var value = TypeConverter.ToString(argument);
            reference.ReturnValue = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLDialogElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}