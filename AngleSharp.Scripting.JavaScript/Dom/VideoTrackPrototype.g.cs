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

    sealed partial class VideoTrackPrototype : VideoTrackInstance
    {
        public VideoTrackPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("id", Engine.AsProperty(GetId));
            FastSetProperty("kind", Engine.AsProperty(GetKind));
            FastSetProperty("label", Engine.AsProperty(GetLabel));
            FastSetProperty("language", Engine.AsProperty(GetLanguage));
            FastSetProperty("selected", Engine.AsProperty(GetSelected, SetSelected));
        }

        public static VideoTrackPrototype CreatePrototypeObject(EngineInstance engine, VideoTrackConstructor constructor)
        {
            var obj = new VideoTrackPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            return Engine.Select(reference.Id);
        }


        JsValue GetKind(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            return Engine.Select(reference.Kind);
        }


        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            return Engine.Select(reference.Label);
        }


        JsValue GetLanguage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            return Engine.Select(reference.Language);
        }


        JsValue GetSelected(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            return Engine.Select(reference.IsSelected);
        }

        void SetSelected(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<VideoTrackInstance>(Fail).RefVideoTrack;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsSelected = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object VideoTrack]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}