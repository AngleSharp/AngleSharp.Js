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

    sealed partial class HTMLSourceElementPrototype : HTMLSourceElementInstance
    {
        public HTMLSourceElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("srcset", Engine.AsProperty(GetSrcset, SetSrcset));
            FastSetProperty("sizes", Engine.AsProperty(GetSizes, SetSizes));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
        }

        public static HTMLSourceElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLSourceElementConstructor constructor)
        {
            var obj = new HTMLSourceElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            return Engine.Select(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetSrcset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            return Engine.Select(reference.SourceSet);
        }

        void SetSrcset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            var value = TypeConverter.ToString(argument);
            reference.SourceSet = value;
        }

        JsValue GetSizes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            return Engine.Select(reference.Sizes);
        }

        void SetSizes(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            var value = TypeConverter.ToString(argument);
            reference.Sizes = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            return Engine.Select(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLSourceElementInstance>(Fail).RefHTMLSourceElement;
            var value = TypeConverter.ToString(argument);
            reference.Media = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLSourceElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}