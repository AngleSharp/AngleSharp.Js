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

    sealed partial class TextTrackPrototype : TextTrackInstance
    {
        public TextTrackPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("addCue", Engine.AsValue(AddCue), true, true, true);
            FastAddProperty("removeCue", Engine.AsValue(RemoveCue), true, true, true);
            FastSetProperty("kind", Engine.AsProperty(GetKind));
            FastSetProperty("label", Engine.AsProperty(GetLabel));
            FastSetProperty("language", Engine.AsProperty(GetLanguage));
            FastSetProperty("mode", Engine.AsProperty(GetMode, SetMode));
            FastSetProperty("cues", Engine.AsProperty(GetCues));
            FastSetProperty("activeCues", Engine.AsProperty(GetActiveCues));
            FastSetProperty("oncuechange", Engine.AsProperty(GetCueChangedEvent, SetCueChangedEvent));
        }

        public static TextTrackPrototype CreatePrototypeObject(EngineInstance engine, TextTrackConstructor constructor)
        {
            var obj = new TextTrackPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue AddCue(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            var cue = DomTypeConverter.ToTextTrackCue(arguments.At(0));
            reference.Add(cue);
            return JsValue.Undefined;
        }

        JsValue RemoveCue(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            var cue = DomTypeConverter.ToTextTrackCue(arguments.At(0));
            reference.Remove(cue);
            return JsValue.Undefined;
        }

        JsValue GetKind(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.Kind);
        }


        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.Label);
        }


        JsValue GetLanguage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.Language);
        }


        JsValue GetMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.Mode);
        }

        void SetMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            var value = DomTypeConverter.ToTextTrackMode(argument);
            reference.Mode = value;
        }

        JsValue GetCues(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.Cues);
        }


        JsValue GetActiveCues(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackInstance>(Fail).RefTextTrack;
            return Engine.Select(reference.ActiveCues);
        }


        JsValue GetCueChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<TextTrackInstance>(Fail);
            return instance.Get("oncuechange");
        }

        void SetCueChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<TextTrackInstance>(Fail);
            var reference = instance.RefTextTrack;
            var existing = GetCueChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CueChanged -= method;
            }

            instance.Put("oncuechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CueChanged += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TextTrack]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}