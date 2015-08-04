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

    sealed partial class HTMLEmbedElementPrototype : HTMLEmbedElementInstance
    {
        public HTMLEmbedElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
        }

        public static HTMLEmbedElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLEmbedElementConstructor constructor)
        {
            var obj = new HTMLEmbedElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            return Engine.Select(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            return Engine.Select(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            var value = TypeConverter.ToString(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            return Engine.Select(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLEmbedElementInstance>(Fail).RefHTMLEmbedElement;
            var value = TypeConverter.ToString(argument);
            reference.DisplayHeight = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLEmbedElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}