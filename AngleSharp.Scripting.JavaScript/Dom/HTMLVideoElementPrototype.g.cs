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
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("videoWidth", Engine.AsProperty(GetVideoWidth));
            FastSetProperty("videoHeight", Engine.AsProperty(GetVideoHeight));
            FastSetProperty("poster", Engine.AsProperty(GetPoster, SetPoster));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
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

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            reference.Remove();
            return JsValue.Undefined;
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

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLVideoElementInstance>(Fail).RefHTMLVideoElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
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