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

    sealed partial class HTMLVideoElementPrototype : HTMLVideoElementInstance
    {
        public HTMLVideoElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("videoWidth", Engine.AsProperty(GetVideoWidth));
            FastSetProperty("videoHeight", Engine.AsProperty(GetVideoHeight));
            FastSetProperty("poster", Engine.AsProperty(GetPoster, SetPoster));
        }

        public static HTMLVideoElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLVideoElementConstructor constructor)
        {
            var obj = new HTMLVideoElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLMediaElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayHeight = value;
        }

        JsValue GetVideoWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.OriginalWidth);
        }


        JsValue GetVideoHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.OriginalHeight);
        }


        JsValue GetPoster(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.Poster);
        }

        void SetPoster(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var value = TypeConverter.ToString(argument);
            reference.Poster = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLVideoElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}