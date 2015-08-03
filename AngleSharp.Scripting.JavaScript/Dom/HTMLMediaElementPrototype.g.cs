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

    sealed partial class HTMLMediaElementPrototype : HTMLMediaElementInstance
    {
        public HTMLMediaElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("load", Engine.AsValue(Load), true, true, true);
            FastAddProperty("canPlayType", Engine.AsValue(CanPlayType), true, true, true);
            FastAddProperty("addTextTrack", Engine.AsValue(AddTextTrack), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("crossOrigin", Engine.AsProperty(GetCrossOrigin, SetCrossOrigin));
            FastSetProperty("preload", Engine.AsProperty(GetPreload, SetPreload));
            FastSetProperty("mediaGroup", Engine.AsProperty(GetMediaGroup, SetMediaGroup));
            FastSetProperty("networkState", Engine.AsProperty(GetNetworkState));
            FastSetProperty("seeking", Engine.AsProperty(GetSeeking));
            FastSetProperty("currentSrc", Engine.AsProperty(GetCurrentSrc));
            FastSetProperty("error", Engine.AsProperty(GetError));
            FastSetProperty("controller", Engine.AsProperty(GetController));
            FastSetProperty("ended", Engine.AsProperty(GetEnded));
            FastSetProperty("autoplay", Engine.AsProperty(GetAutoplay, SetAutoplay));
            FastSetProperty("loop", Engine.AsProperty(GetLoop, SetLoop));
            FastSetProperty("controls", Engine.AsProperty(GetControls, SetControls));
            FastSetProperty("defaultMuted", Engine.AsProperty(GetDefaultMuted, SetDefaultMuted));
            FastSetProperty("startDate", Engine.AsProperty(GetStartDate));
            FastSetProperty("audioTracks", Engine.AsProperty(GetAudioTracks));
            FastSetProperty("videoTracks", Engine.AsProperty(GetVideoTracks));
            FastSetProperty("textTracks", Engine.AsProperty(GetTextTracks));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLMediaElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMediaElementConstructor constructor)
        {
            var obj = new HTMLMediaElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Load(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            reference.Load();
            return JsValue.Undefined;
        }

        JsValue CanPlayType(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var type = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CanPlayType(type));
        }

        JsValue AddTextTrack(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var kind = TypeConverter.ToString(arguments.At(0));
            var label = TypeConverter.ToString(arguments.At(1));
            var language = TypeConverter.ToString(arguments.At(2));
            return Engine.Select(reference.AddTextTrack(kind, label, language));
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetCrossOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.CrossOrigin);
        }

        void SetCrossOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToString(argument);
            reference.CrossOrigin = value;
        }

        JsValue GetPreload(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Preload);
        }

        void SetPreload(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToString(argument);
            reference.Preload = value;
        }

        JsValue GetMediaGroup(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.MediaGroup);
        }

        void SetMediaGroup(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToString(argument);
            reference.MediaGroup = value;
        }

        JsValue GetNetworkState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.NetworkState);
        }


        JsValue GetSeeking(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsSeeking);
        }


        JsValue GetCurrentSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.CurrentSource);
        }


        JsValue GetError(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Error);
        }


        JsValue GetController(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Controller);
        }


        JsValue GetEnded(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsEnded);
        }


        JsValue GetAutoplay(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsAutoplay);
        }

        void SetAutoplay(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsAutoplay = value;
        }

        JsValue GetLoop(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsLoop);
        }

        void SetLoop(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsLoop = value;
        }

        JsValue GetControls(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsShowingControls);
        }

        void SetControls(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsShowingControls = value;
        }

        JsValue GetDefaultMuted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.IsDefaultMuted);
        }

        void SetDefaultMuted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDefaultMuted = value;
        }

        JsValue GetStartDate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.StartDate);
        }


        JsValue GetAudioTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.AudioTracks);
        }


        JsValue GetVideoTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.VideoTracks);
        }


        JsValue GetTextTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.TextTracks);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLMediaElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}