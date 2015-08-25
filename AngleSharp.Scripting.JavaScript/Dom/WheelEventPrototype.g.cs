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

    sealed partial class WheelEventPrototype : WheelEventInstance
    {
        readonly EngineInstance _engine;

        public WheelEventPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("initWheelEvent", Engine.AsValue(InitWheelEvent), true, true, true);
            FastAddProperty("getModifierState", Engine.AsValue(GetModifierState), true, true, true);
            FastAddProperty("initMouseEvent", Engine.AsValue(InitMouseEvent), true, true, true);
            FastAddProperty("initUIEvent", Engine.AsValue(InitUIEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("deltaX", Engine.AsProperty(GetDeltaX));
            FastSetProperty("deltaY", Engine.AsProperty(GetDeltaY));
            FastSetProperty("deltaZ", Engine.AsProperty(GetDeltaZ));
            FastSetProperty("deltaMode", Engine.AsProperty(GetDeltaMode));
            FastSetProperty("screenX", Engine.AsProperty(GetScreenX));
            FastSetProperty("screenY", Engine.AsProperty(GetScreenY));
            FastSetProperty("clientX", Engine.AsProperty(GetClientX));
            FastSetProperty("clientY", Engine.AsProperty(GetClientY));
            FastSetProperty("ctrlKey", Engine.AsProperty(GetCtrlKey));
            FastSetProperty("shiftKey", Engine.AsProperty(GetShiftKey));
            FastSetProperty("altKey", Engine.AsProperty(GetAltKey));
            FastSetProperty("metaKey", Engine.AsProperty(GetMetaKey));
            FastSetProperty("button", Engine.AsProperty(GetButton));
            FastSetProperty("buttons", Engine.AsProperty(GetButtons));
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

        public static WheelEventPrototype CreatePrototypeObject(EngineInstance engine, WheelEventConstructor constructor)
        {
            var obj = new WheelEventPrototype(engine)
            {
                Prototype = engine.Constructors.MouseEvent.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InitWheelEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var view = DomTypeConverter.ToWindow(arguments.At(3));
            var detail = TypeConverter.ToInt32(arguments.At(4));
            var screenX = TypeConverter.ToInt32(arguments.At(5));
            var screenY = TypeConverter.ToInt32(arguments.At(6));
            var clientX = TypeConverter.ToInt32(arguments.At(7));
            var clientY = TypeConverter.ToInt32(arguments.At(8));
            var button = DomTypeConverter.ToMouseButton(arguments.At(9));
            var target = DomTypeConverter.ToEventTarget(arguments.At(10));
            var modifiersList = TypeConverter.ToString(arguments.At(11));
            var deltaX = TypeConverter.ToNumber(arguments.At(12));
            var deltaY = TypeConverter.ToNumber(arguments.At(13));
            var deltaZ = TypeConverter.ToNumber(arguments.At(14));
            var deltaMode = DomTypeConverter.ToWheelMode(arguments.At(15));
            reference.Init(type, bubbles, cancelable, view, detail, screenX, screenY, clientX, clientY, button, target, modifiersList, deltaX, deltaY, deltaZ, deltaMode);
            return JsValue.Undefined;
        }

        JsValue GetModifierState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            var key = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.GetModifierState(key));
        }

        JsValue InitMouseEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var view = DomTypeConverter.ToWindow(arguments.At(3));
            var detail = TypeConverter.ToInt32(arguments.At(4));
            var screenX = TypeConverter.ToInt32(arguments.At(5));
            var screenY = TypeConverter.ToInt32(arguments.At(6));
            var clientX = TypeConverter.ToInt32(arguments.At(7));
            var clientY = TypeConverter.ToInt32(arguments.At(8));
            var ctrlKey = TypeConverter.ToBoolean(arguments.At(9));
            var altKey = TypeConverter.ToBoolean(arguments.At(10));
            var shiftKey = TypeConverter.ToBoolean(arguments.At(11));
            var metaKey = TypeConverter.ToBoolean(arguments.At(12));
            var button = DomTypeConverter.ToMouseButton(arguments.At(13));
            var target = DomTypeConverter.ToEventTarget(arguments.At(14));
            reference.Init(type, bubbles, cancelable, view, detail, screenX, screenY, clientX, clientY, ctrlKey, altKey, shiftKey, metaKey, button, target);
            return JsValue.Undefined;
        }

        JsValue InitUIEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
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
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetDeltaX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.DeltaX);
        }


        JsValue GetDeltaY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.DeltaY);
        }


        JsValue GetDeltaZ(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.DeltaZ);
        }


        JsValue GetDeltaMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.DeltaMode);
        }


        JsValue GetScreenX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.ScreenX);
        }


        JsValue GetScreenY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.ScreenY);
        }


        JsValue GetClientX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.ClientX);
        }


        JsValue GetClientY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.ClientY);
        }


        JsValue GetCtrlKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsCtrlPressed);
        }


        JsValue GetShiftKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsShiftPressed);
        }


        JsValue GetAltKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsAltPressed);
        }


        JsValue GetMetaKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsMetaPressed);
        }


        JsValue GetButton(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Button);
        }


        JsValue GetButtons(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Buttons);
        }


        JsValue GetRelatedTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Target);
        }


        JsValue GetView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.View);
        }


        JsValue GetDetail(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Detail);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<WheelEventInstance>(Fail).RefWheelEvent;
            return _engine.GetDomNode(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object WheelEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}