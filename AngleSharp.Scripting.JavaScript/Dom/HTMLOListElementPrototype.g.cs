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

    sealed partial class HTMLOListElementPrototype : HTMLOListElementInstance
    {
        public HTMLOListElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("reversed", Engine.AsProperty(GetReversed, SetReversed));
            FastSetProperty("start", Engine.AsProperty(GetStart, SetStart));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
        }

        public static HTMLOListElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLOListElementConstructor constructor)
        {
            var obj = new HTMLOListElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetReversed(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            return Engine.Select(reference.IsReversed);
        }

        void SetReversed(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsReversed = value;
        }

        JsValue GetStart(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            return Engine.Select(reference.Start);
        }

        void SetStart(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Start = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOListElementInstance>(Fail).RefHTMLOListElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLOListElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}