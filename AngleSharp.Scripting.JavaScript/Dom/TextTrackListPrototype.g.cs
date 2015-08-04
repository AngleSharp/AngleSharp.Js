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

    sealed partial class TextTrackListPrototype : TextTrackListInstance
    {
        public TextTrackListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("onaddtrack", Engine.AsProperty(GetTrackAddedEvent, SetTrackAddedEvent));
            FastSetProperty("onremovetrack", Engine.AsProperty(GetTrackRemovedEvent, SetTrackRemovedEvent));
        }

        public static TextTrackListPrototype CreatePrototypeObject(EngineInstance engine, TextTrackListConstructor constructor)
        {
            var obj = new TextTrackListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackListInstance>(Fail).RefTextTrackList;
            return Engine.Select(reference.Length);
        }


        JsValue GetTrackAddedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<TextTrackListInstance>(Fail);
            return instance.Get("onaddtrack");
        }

        void SetTrackAddedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<TextTrackListInstance>(Fail);
            var reference = instance.RefTextTrackList;
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
            var instance = thisObj.TryCast<TextTrackListInstance>(Fail);
            return instance.Get("onremovetrack");
        }

        void SetTrackRemovedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<TextTrackListInstance>(Fail);
            var reference = instance.RefTextTrackList;
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
            return "[object TextTrackList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}