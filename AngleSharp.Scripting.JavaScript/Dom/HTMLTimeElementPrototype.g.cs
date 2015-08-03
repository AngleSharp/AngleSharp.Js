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

    sealed partial class HTMLTimeElementPrototype : HTMLTimeElementInstance
    {
        public HTMLTimeElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("datetime", Engine.AsProperty(GetDatetime, SetDatetime));
        }

        public static HTMLTimeElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTimeElementConstructor constructor)
        {
            var obj = new HTMLTimeElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDatetime(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTimeElementInstance>(Fail).RefHTMLTimeElement;
            return Engine.Select(reference.DateTime);
        }

        void SetDatetime(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTimeElementInstance>(Fail).RefHTMLTimeElement;
            var value = TypeConverter.ToString(argument);
            reference.DateTime = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTimeElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}