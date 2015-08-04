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

    sealed partial class HTMLModElementPrototype : HTMLModElementInstance
    {
        public HTMLModElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("cite", Engine.AsProperty(GetCite, SetCite));
            FastSetProperty("datetime", Engine.AsProperty(GetDatetime, SetDatetime));
        }

        public static HTMLModElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLModElementConstructor constructor)
        {
            var obj = new HTMLModElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCite(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLModElementInstance>(Fail).RefHTMLModElement;
            return Engine.Select(reference.Citation);
        }

        void SetCite(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLModElementInstance>(Fail).RefHTMLModElement;
            var value = TypeConverter.ToString(argument);
            reference.Citation = value;
        }

        JsValue GetDatetime(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLModElementInstance>(Fail).RefHTMLModElement;
            return Engine.Select(reference.DateTime);
        }

        void SetDatetime(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLModElementInstance>(Fail).RefHTMLModElement;
            var value = TypeConverter.ToString(argument);
            reference.DateTime = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLModElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}