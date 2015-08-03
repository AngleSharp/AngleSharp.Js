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

    sealed partial class HTMLOptGroupElementPrototype : HTMLOptGroupElementInstance
    {
        public HTMLOptGroupElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
        }

        public static HTMLOptGroupElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLOptGroupElementConstructor constructor)
        {
            var obj = new HTMLOptGroupElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptGroupElementInstance>(Fail).RefHTMLOptGroupElement;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptGroupElementInstance>(Fail).RefHTMLOptGroupElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptGroupElementInstance>(Fail).RefHTMLOptGroupElement;
            return Engine.Select(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptGroupElementInstance>(Fail).RefHTMLOptGroupElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLOptGroupElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}