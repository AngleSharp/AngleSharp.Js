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

    sealed partial class VideoTrackListPrototype : VideoTrackListInstance
    {
        public VideoTrackListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getTrackById", Engine.AsValue(GetTrackById), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("selectedIndex", Engine.AsProperty(GetSelectedIndex));
            FastSetProperty("onchange", Engine.AsProperty(GetChangedEvent, SetChangedEvent));
            FastSetProperty("onaddtrack", Engine.AsProperty(GetTrackAddedEvent, SetTrackAddedEvent));
            FastSetProperty("onremovetrack", Engine.AsProperty(GetTrackRemovedEvent, SetTrackRemovedEvent));
        }

        public static VideoTrackListPrototype CreatePrototypeObject(EngineInstance engine, VideoTrackListConstructor constructor)
        {
            var obj = new VideoTrackListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetTrackById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<VideoTrackListInstance>(Fail).RefVideoTrackList;
            var id = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetTrackById(id));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackListInstance>(Fail).RefVideoTrackList;
            return Engine.Select(reference.Length);
        }


        JsValue GetSelectedIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<VideoTrackListInstance>(Fail).RefVideoTrackList;
            return Engine.Select(reference.SelectedIndex);
        }


        JsValue GetChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            return instance.Get("onchange");
        }

        void SetChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            var reference = instance.RefVideoTrackList;
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
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            return instance.Get("onaddtrack");
        }

        void SetTrackAddedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            var reference = instance.RefVideoTrackList;
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
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            return instance.Get("onremovetrack");
        }

        void SetTrackRemovedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<VideoTrackListInstance>(Fail);
            var reference = instance.RefVideoTrackList;
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
            return "[object VideoTrackList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}