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

    sealed partial class MessageEventPrototype : MessageEventInstance
    {
        public MessageEventPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("initMessageEvent", Engine.AsValue(InitMessageEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("data", Engine.AsProperty(GetData));
            FastSetProperty("origin", Engine.AsProperty(GetOrigin));
            FastSetProperty("lastEventId", Engine.AsProperty(GetLastEventId));
            FastSetProperty("source", Engine.AsProperty(GetSource));
            FastSetProperty("ports", Engine.AsProperty(GetPorts));
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

        public static MessageEventPrototype CreatePrototypeObject(EngineInstance engine, MessageEventConstructor constructor)
        {
            var obj = new MessageEventPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Event.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InitMessageEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var data = SystemTypeConverter.ToObject(arguments.At(3));
            var origin = TypeConverter.ToString(arguments.At(4));
            var lastEventId = TypeConverter.ToString(arguments.At(5));
            var source = DomTypeConverter.ToWindow(arguments.At(6));
            var ports = new AngleSharp.Dom.Events.IMessagePort[Math.Max(0, arguments.Length - 7)];

            for (var i = 0; i < ports.Length; i++)
                ports[i] = DomTypeConverter.ToMessagePort(arguments.At(i + 7));

            reference.Init(type, bubbles, cancelable, data, origin, lastEventId, source, ports);
            return JsValue.Undefined;
        }

        JsValue StopPropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetData(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Data);
        }


        JsValue GetOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Origin);
        }


        JsValue GetLastEventId(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.LastEventId);
        }


        JsValue GetSource(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Source);
        }


        JsValue GetPorts(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Ports);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MessageEventInstance>(Fail).RefMessageEvent;
            return Engine.Select(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MessageEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}