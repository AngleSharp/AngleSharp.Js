namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MediaControllerPrototype : MediaControllerInstance
    {
        readonly EngineInstance _engine;

        public MediaControllerPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("play", Engine.AsValue(Play), true, true, true);
            FastAddProperty("pause", Engine.AsValue(Pause), true, true, true);
            FastSetProperty("buffered", Engine.AsProperty(GetBuffered));
            FastSetProperty("seekable", Engine.AsProperty(GetSeekable));
            FastSetProperty("played", Engine.AsProperty(GetPlayed));
            FastSetProperty("duration", Engine.AsProperty(GetDuration));
            FastSetProperty("currentTime", Engine.AsProperty(GetCurrentTime, SetCurrentTime));
            FastSetProperty("defaultPlaybackRate", Engine.AsProperty(GetDefaultPlaybackRate, SetDefaultPlaybackRate));
            FastSetProperty("playbackRate", Engine.AsProperty(GetPlaybackRate, SetPlaybackRate));
            FastSetProperty("volume", Engine.AsProperty(GetVolume, SetVolume));
            FastSetProperty("muted", Engine.AsProperty(GetMuted, SetMuted));
            FastSetProperty("paused", Engine.AsProperty(GetPaused));
            FastSetProperty("readyState", Engine.AsProperty(GetReadyState));
            FastSetProperty("playbackState", Engine.AsProperty(GetPlaybackState));
            FastSetProperty("onemptied", Engine.AsProperty(GetEmptiedEvent, SetEmptiedEvent));
            FastSetProperty("onloadedmetadata", Engine.AsProperty(GetLoadedMetadataEvent, SetLoadedMetadataEvent));
            FastSetProperty("onloadeddata", Engine.AsProperty(GetLoadedDataEvent, SetLoadedDataEvent));
            FastSetProperty("oncanplay", Engine.AsProperty(GetCanPlayEvent, SetCanPlayEvent));
            FastSetProperty("oncanplaythrough", Engine.AsProperty(GetCanPlayThroughEvent, SetCanPlayThroughEvent));
            FastSetProperty("onended", Engine.AsProperty(GetEndedEvent, SetEndedEvent));
            FastSetProperty("onwaiting", Engine.AsProperty(GetWaitingEvent, SetWaitingEvent));
            FastSetProperty("ondurationchange", Engine.AsProperty(GetDurationChangedEvent, SetDurationChangedEvent));
            FastSetProperty("ontimeupdate", Engine.AsProperty(GetTimeUpdatedEvent, SetTimeUpdatedEvent));
            FastSetProperty("onpause", Engine.AsProperty(GetPausedEvent, SetPausedEvent));
            FastSetProperty("onplay", Engine.AsProperty(GetPlayedEvent, SetPlayedEvent));
            FastSetProperty("onplaying", Engine.AsProperty(GetPlayingEvent, SetPlayingEvent));
            FastSetProperty("onratechange", Engine.AsProperty(GetRateChangedEvent, SetRateChangedEvent));
            FastSetProperty("onvolumechange", Engine.AsProperty(GetVolumeChangedEvent, SetVolumeChangedEvent));
        }

        public static MediaControllerPrototype CreatePrototypeObject(EngineInstance engine, MediaControllerConstructor constructor)
        {
            var obj = new MediaControllerPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Play(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            reference.Play();
            return JsValue.Undefined;
        }

        JsValue Pause(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            reference.Pause();
            return JsValue.Undefined;
        }

        JsValue GetBuffered(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.BufferedTime);
        }


        JsValue GetSeekable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.SeekableTime);
        }


        JsValue GetPlayed(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.PlayedTime);
        }


        JsValue GetDuration(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.Duration);
        }


        JsValue GetCurrentTime(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.CurrentTime);
        }

        void SetCurrentTime(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            var value = TypeConverter.ToNumber(argument);
            reference.CurrentTime = value;
        }

        JsValue GetDefaultPlaybackRate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.DefaultPlaybackRate);
        }

        void SetDefaultPlaybackRate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            var value = TypeConverter.ToNumber(argument);
            reference.DefaultPlaybackRate = value;
        }

        JsValue GetPlaybackRate(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.PlaybackRate);
        }

        void SetPlaybackRate(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            var value = TypeConverter.ToNumber(argument);
            reference.PlaybackRate = value;
        }

        JsValue GetVolume(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.Volume);
        }

        void SetVolume(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            var value = TypeConverter.ToNumber(argument);
            reference.Volume = value;
        }

        JsValue GetMuted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.IsMuted);
        }

        void SetMuted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsMuted = value;
        }

        JsValue GetPaused(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.IsPaused);
        }


        JsValue GetReadyState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.ReadyState);
        }


        JsValue GetPlaybackState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaControllerInstance>(Fail).RefMediaController;
            return _engine.GetDomNode(reference.PlaybackState);
        }


        JsValue GetEmptiedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onemptied");
        }

        void SetEmptiedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetEmptiedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Emptied -= method;
            }

            instance.Put("onemptied", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Emptied += method;
            }
        }

        JsValue GetLoadedMetadataEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onloadedmetadata");
        }

        void SetLoadedMetadataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetLoadedMetadataEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.LoadedMetadata -= method;
            }

            instance.Put("onloadedmetadata", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.LoadedMetadata += method;
            }
        }

        JsValue GetLoadedDataEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onloadeddata");
        }

        void SetLoadedDataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetLoadedDataEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.LoadedData -= method;
            }

            instance.Put("onloadeddata", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.LoadedData += method;
            }
        }

        JsValue GetCanPlayEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("oncanplay");
        }

        void SetCanPlayEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetCanPlayEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CanPlay -= method;
            }

            instance.Put("oncanplay", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CanPlay += method;
            }
        }

        JsValue GetCanPlayThroughEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("oncanplaythrough");
        }

        void SetCanPlayThroughEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetCanPlayThroughEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CanPlayThrough -= method;
            }

            instance.Put("oncanplaythrough", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CanPlayThrough += method;
            }
        }

        JsValue GetEndedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onended");
        }

        void SetEndedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetEndedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Ended -= method;
            }

            instance.Put("onended", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Ended += method;
            }
        }

        JsValue GetWaitingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onwaiting");
        }

        void SetWaitingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetWaitingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Waiting -= method;
            }

            instance.Put("onwaiting", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Waiting += method;
            }
        }

        JsValue GetDurationChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("ondurationchange");
        }

        void SetDurationChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetDurationChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DurationChanged -= method;
            }

            instance.Put("ondurationchange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DurationChanged += method;
            }
        }

        JsValue GetTimeUpdatedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("ontimeupdate");
        }

        void SetTimeUpdatedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetTimeUpdatedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.TimeUpdated -= method;
            }

            instance.Put("ontimeupdate", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.TimeUpdated += method;
            }
        }

        JsValue GetPausedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onpause");
        }

        void SetPausedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetPausedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Paused -= method;
            }

            instance.Put("onpause", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Paused += method;
            }
        }

        JsValue GetPlayedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onplay");
        }

        void SetPlayedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetPlayedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Played -= method;
            }

            instance.Put("onplay", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Played += method;
            }
        }

        JsValue GetPlayingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onplaying");
        }

        void SetPlayingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetPlayingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Playing -= method;
            }

            instance.Put("onplaying", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Playing += method;
            }
        }

        JsValue GetRateChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onratechange");
        }

        void SetRateChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetRateChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.RateChanged -= method;
            }

            instance.Put("onratechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.RateChanged += method;
            }
        }

        JsValue GetVolumeChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            return instance.Get("onvolumechange");
        }

        void SetVolumeChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MediaControllerInstance>(Fail);
            var reference = instance.RefMediaController;
            var existing = GetVolumeChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.VolumeChanged -= method;
            }

            instance.Put("onvolumechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.VolumeChanged += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MediaController]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}