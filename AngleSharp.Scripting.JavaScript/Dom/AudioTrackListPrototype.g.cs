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

    sealed partial class AudioTrackListPrototype : AudioTrackListInstance
    {
        public AudioTrackListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getTrackById", Engine.AsValue(GetTrackById), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("onchange", Engine.AsProperty(GetChangedEvent, SetChangedEvent));
            FastSetProperty("onaddtrack", Engine.AsProperty(GetTrackAddedEvent, SetTrackAddedEvent));
            FastSetProperty("onremovetrack", Engine.AsProperty(GetTrackRemovedEvent, SetTrackRemovedEvent));
        }

        public static AudioTrackListPrototype CreatePrototypeObject(EngineInstance engine, AudioTrackListConstructor constructor)
        {
            var obj = new AudioTrackListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetTrackById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<AudioTrackListInstance>(Fail).RefAudioTrackList;
            var id = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetTrackById(id));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackListInstance>(Fail).RefAudioTrackList;
            return Engine.Select(reference.Length);
        }


        JsValue GetChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            return instance.Get("onchange");
        }

        void SetChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            var reference = instance.RefAudioTrackList;
            var existing = GetChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Changed -= method;
            }

            instance.Put("onchange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Changed += method;
            }
        }

        JsValue GetTrackAddedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            return instance.Get("onaddtrack");
        }

        void SetTrackAddedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            var reference = instance.RefAudioTrackList;
            var existing = GetTrackAddedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.TrackAdded -= method;
            }

            instance.Put("onaddtrack", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.TrackAdded += method;
            }
        }

        JsValue GetTrackRemovedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            return instance.Get("onremovetrack");
        }

        void SetTrackRemovedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<AudioTrackListInstance>(Fail);
            var reference = instance.RefAudioTrackList;
            var existing = GetTrackRemovedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.TrackRemoved -= method;
            }

            instance.Put("onremovetrack", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.TrackRemoved += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object AudioTrackList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}