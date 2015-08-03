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

    sealed partial class HTMLQuoteElementPrototype : HTMLQuoteElementInstance
    {
        public HTMLQuoteElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("cite", Engine.AsProperty(GetCite, SetCite));
        }

        public static HTMLQuoteElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLQuoteElementConstructor constructor)
        {
            var obj = new HTMLQuoteElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCite(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLQuoteElementInstance>(Fail).RefHTMLQuoteElement;
            return Engine.Select(reference.Citation);
        }

        void SetCite(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLQuoteElementInstance>(Fail).RefHTMLQuoteElement;
            var value = TypeConverter.ToString(argument);
            reference.Citation = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLQuoteElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}