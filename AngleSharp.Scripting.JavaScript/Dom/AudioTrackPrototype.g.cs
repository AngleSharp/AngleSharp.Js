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

    sealed partial class AudioTrackPrototype : AudioTrackInstance
    {
        public AudioTrackPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("id", Engine.AsProperty(GetId));
            FastSetProperty("kind", Engine.AsProperty(GetKind));
            FastSetProperty("label", Engine.AsProperty(GetLabel));
            FastSetProperty("language", Engine.AsProperty(GetLanguage));
            FastSetProperty("enabled", Engine.AsProperty(GetEnabled, SetEnabled));
        }

        public static AudioTrackPrototype CreatePrototypeObject(EngineInstance engine, AudioTrackConstructor constructor)
        {
            var obj = new AudioTrackPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            return Engine.Select(reference.Id);
        }


        JsValue GetKind(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            return Engine.Select(reference.Kind);
        }


        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            return Engine.Select(reference.Label);
        }


        JsValue GetLanguage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            return Engine.Select(reference.Language);
        }


        JsValue GetEnabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            return Engine.Select(reference.IsEnabled);
        }

        void SetEnabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<AudioTrackInstance>(Fail).RefAudioTrack;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsEnabled = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object AudioTrack]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}