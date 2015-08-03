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

    sealed partial class HTMLTableColElementPrototype : HTMLTableColElementInstance
    {
        public HTMLTableColElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("span", Engine.AsProperty(GetSpan, SetSpan));
        }

        public static HTMLTableColElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableColElementConstructor constructor)
        {
            var obj = new HTMLTableColElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSpan(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableColElementInstance>(Fail).RefHTMLTableColElement;
            return Engine.Select(reference.Span);
        }

        void SetSpan(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableColElementInstance>(Fail).RefHTMLTableColElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Span = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableColElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}