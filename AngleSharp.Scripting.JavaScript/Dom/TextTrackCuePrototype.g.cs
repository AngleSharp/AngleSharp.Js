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

    sealed partial class TextTrackCuePrototype : TextTrackCueInstance
    {
        readonly EngineInstance _engine;

        public TextTrackCuePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getCueAsHTML", Engine.AsValue(GetCueAsHTML), true, true, true);
            FastSetProperty("id", Engine.AsProperty(GetId, SetId));
            FastSetProperty("track", Engine.AsProperty(GetTrack));
            FastSetProperty("startTime", Engine.AsProperty(GetStartTime, SetStartTime));
            FastSetProperty("endTime", Engine.AsProperty(GetEndTime, SetEndTime));
            FastSetProperty("pauseOnExit", Engine.AsProperty(GetPauseOnExit, SetPauseOnExit));
            FastSetProperty("vertical", Engine.AsProperty(GetVertical, SetVertical));
            FastSetProperty("snapToLines", Engine.AsProperty(GetSnapToLines, SetSnapToLines));
            FastSetProperty("line", Engine.AsProperty(GetLine, SetLine));
            FastSetProperty("position", Engine.AsProperty(GetPosition, SetPosition));
            FastSetProperty("size", Engine.AsProperty(GetSize, SetSize));
            FastSetProperty("align", Engine.AsProperty(GetAlign, SetAlign));
            FastSetProperty("text", Engine.AsProperty(GetText, SetText));
            FastSetProperty("onenter", Engine.AsProperty(GetOnenter, SetOnenter));
            FastSetProperty("onexit", Engine.AsProperty(GetOnexit, SetOnexit));
        }

        public static TextTrackCuePrototype CreatePrototypeObject(EngineInstance engine, TextTrackCueConstructor constructor)
        {
            var obj = new TextTrackCuePrototype(engine)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCueAsHTML(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.AsHtml());
        }

        JsValue GetId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Id);
        }

        void SetId(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToString(argument);
            reference.Id = value;
        }

        JsValue GetTrack(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Track);
        }


        JsValue GetStartTime(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.StartTime);
        }

        void SetStartTime(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToNumber(argument);
            reference.StartTime = value;
        }

        JsValue GetEndTime(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.EndTime);
        }

        void SetEndTime(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToNumber(argument);
            reference.EndTime = value;
        }

        JsValue GetPauseOnExit(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.IsPausedOnExit);
        }

        void SetPauseOnExit(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsPausedOnExit = value;
        }

        JsValue GetVertical(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Vertical);
        }

        void SetVertical(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToString(argument);
            reference.Vertical = value;
        }

        JsValue GetSnapToLines(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.IsSnappedToLines);
        }

        void SetSnapToLines(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsSnappedToLines = value;
        }

        JsValue GetLine(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Line);
        }

        void SetLine(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToInt32(argument);
            reference.Line = value;
        }

        JsValue GetPosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Position);
        }

        void SetPosition(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToInt32(argument);
            reference.Position = value;
        }

        JsValue GetSize(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Size);
        }

        void SetSize(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToInt32(argument);
            reference.Size = value;
        }

        JsValue GetAlign(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Alignment);
        }

        void SetAlign(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToString(argument);
            reference.Alignment = value;
        }

        JsValue GetText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Text);
        }

        void SetText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = TypeConverter.ToString(argument);
            reference.Text = value;
        }

        JsValue GetOnenter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Entered);
        }

        void SetOnenter(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = DomTypeConverter.ToEventHandler(argument);
            reference.Entered = value;
        }

        JsValue GetOnexit(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            return _engine.GetDomNode(reference.Exited);
        }

        void SetOnexit(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TextTrackCueInstance>(Fail).RefTextTrackCue;
            var value = DomTypeConverter.ToEventHandler(argument);
            reference.Exited = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TextTrackCue]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}