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
        readonly EngineInstance _engine;

        public HTMLMediaElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("load", Engine.AsValue(Load), true, true, true);
            FastAddProperty("canPlayType", Engine.AsValue(CanPlayType), true, true, true);
            FastAddProperty("addTextTrack", Engine.AsValue(AddTextTrack), true, true, true);
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
        }

        public static HTMLMediaElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMediaElementConstructor constructor)
        {
            var obj = new HTMLMediaElementPrototype(engine)
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
            return _engine.GetDomNode(reference.CanPlayType(type));
        }

        JsValue AddTextTrack(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            var kind = TypeConverter.ToString(arguments.At(0));
            var label = TypeConverter.ToString(arguments.At(1));
            var language = TypeConverter.ToString(arguments.At(2));
            return _engine.GetDomNode(reference.AddTextTrack(kind, label, language));
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.Source);
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
            return _engine.GetDomNode(reference.CrossOrigin);
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
            return _engine.GetDomNode(reference.Preload);
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
            return _engine.GetDomNode(reference.MediaGroup);
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
            return _engine.GetDomNode(reference.NetworkState);
        }


        JsValue GetSeeking(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.IsSeeking);
        }


        JsValue GetCurrentSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.CurrentSource);
        }


        JsValue GetError(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.Error);
        }


        JsValue GetController(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.Controller);
        }


        JsValue GetEnded(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.IsEnded);
        }


        JsValue GetAutoplay(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.IsAutoplay);
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
            return _engine.GetDomNode(reference.IsLoop);
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
            return _engine.GetDomNode(reference.IsShowingControls);
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
            return _engine.GetDomNode(reference.IsDefaultMuted);
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
            return _engine.GetDomNode(reference.StartDate);
        }


        JsValue GetAudioTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.AudioTracks);
        }


        JsValue GetVideoTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.VideoTracks);
        }


        JsValue GetTextTracks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMediaElementInstance>(Fail).RefHTMLMediaElement;
            return _engine.GetDomNode(reference.TextTracks);
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