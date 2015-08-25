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

    sealed partial class FocusEventPrototype : FocusEventInstance
    {
        readonly EngineInstance _engine;

        public FocusEventPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("initFocusEvent", Engine.AsValue(InitFocusEvent), true, true, true);
            FastAddProperty("initUIEvent", Engine.AsValue(InitUIEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("relatedTarget", Engine.AsProperty(GetRelatedTarget));
            FastSetProperty("view", Engine.AsProperty(GetView));
            FastSetProperty("detail", Engine.AsProperty(GetDetail));
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

        public static FocusEventPrototype CreatePrototypeObject(EngineInstance engine, FocusEventConstructor constructor)
        {
            var obj = new FocusEventPrototype(engine)
            {
                Prototype = engine.Constructors.UIEvent.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InitFocusEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var view = DomTypeConverter.ToWindow(arguments.At(3));
            var detail = TypeConverter.ToInt32(arguments.At(4));
            var target = DomTypeConverter.ToEventTarget(arguments.At(5));
            reference.Init(type, bubbles, cancelable, view, detail, target);
            return JsValue.Undefined;
        }

        JsValue InitUIEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var view = DomTypeConverter.ToWindow(arguments.At(3));
            var detail = TypeConverter.ToInt32(arguments.At(4));
            reference.Init(type, bubbles, cancelable, view, detail);
            return JsValue.Undefined;
        }

        JsValue StopPropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetRelatedTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.Target);
        }


        JsValue GetView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.View);
        }


        JsValue GetDetail(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.Detail);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<FocusEventInstance>(Fail).RefFocusEvent;
            return _engine.GetDomNode(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object FocusEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}