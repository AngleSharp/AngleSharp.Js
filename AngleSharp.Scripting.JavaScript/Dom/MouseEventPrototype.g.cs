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

    sealed partial class MouseEventPrototype : MouseEventInstance
    {
        public MouseEventPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getModifierState", Engine.AsValue(GetModifierState), true, true, true);
            FastAddProperty("initMouseEvent", Engine.AsValue(InitMouseEvent), true, true, true);
            FastAddProperty("initUIEvent", Engine.AsValue(InitUIEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
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

        public static MouseEventPrototype CreatePrototypeObject(EngineInstance engine, MouseEventConstructor constructor)
        {
            var obj = new MouseEventPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.UIEvent.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetModifierState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            var key = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetModifierState(key));
        }

        JsValue InitMouseEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
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
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
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
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetScreenX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.ScreenX);
        }


        JsValue GetScreenY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.ScreenY);
        }


        JsValue GetClientX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.ClientX);
        }


        JsValue GetClientY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.ClientY);
        }


        JsValue GetCtrlKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsCtrlPressed);
        }


        JsValue GetShiftKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsShiftPressed);
        }


        JsValue GetAltKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsAltPressed);
        }


        JsValue GetMetaKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsMetaPressed);
        }


        JsValue GetButton(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Button);
        }


        JsValue GetButtons(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Buttons);
        }


        JsValue GetRelatedTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Target);
        }


        JsValue GetView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.View);
        }


        JsValue GetDetail(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Detail);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MouseEventInstance>(Fail).RefMouseEvent;
            return Engine.Select(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MouseEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}