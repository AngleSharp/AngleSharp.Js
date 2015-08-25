namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class ErrorEventPrototype : ErrorEventInstance
    {
        readonly EngineInstance _engine;

        public ErrorEventPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("message", Engine.AsProperty(GetMessage));
            FastSetProperty("filename", Engine.AsProperty(GetFilename));
            FastSetProperty("lineno", Engine.AsProperty(GetLineno));
            FastSetProperty("colno", Engine.AsProperty(GetColno));
            FastSetProperty("error", Engine.AsProperty(GetError));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("target", Engine.AsProperty(GetTarget));
            FastSetProperty("currentTarget", Engine.AsProperty(GetCurrentTarget));
            FastSetProperty("eventPhase", Engine.AsProperty(GetEventPhase));
            FastSetProperty("bubbles", Engine.AsProperty(GetBubbles));
            FastSetProperty("cancelable", Engine.AsProperty(GetCancelable));
            FastSetProperty("defaultPrevented", Engine.AsProperty(GetDefaultPrevented));
            FastSetProperty("isTrusted", Engine.AsProperty(GetIsTrusted, SetIsTrusted));
            FastSetProperty("timeStamp", Engine.AsProperty(GetTimeStamp));
        }

        public static ErrorEventPrototype CreatePrototypeObject(EngineInstance engine, ErrorEventConstructor constructor)
        {
            var obj = new ErrorEventPrototype(engine)
            {
                Prototype = engine.Constructors.Event.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue StopPropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetMessage(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Message);
        }


        JsValue GetFilename(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.FileName);
        }


        JsValue GetLineno(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Line);
        }


        JsValue GetColno(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Column);
        }


        JsValue GetError(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Error);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ErrorEventInstance>(Fail).RefErrorEvent;
            return _engine.GetDomNode(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object ErrorEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}