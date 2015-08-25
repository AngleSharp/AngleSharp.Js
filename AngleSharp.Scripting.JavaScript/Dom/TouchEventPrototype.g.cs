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

    sealed partial class TouchEventPrototype : TouchEventInstance
    {
        readonly EngineInstance _engine;

        public TouchEventPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("initUIEvent", Engine.AsValue(InitUIEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("touches", Engine.AsProperty(GetTouches));
            FastSetProperty("targetTouches", Engine.AsProperty(GetTargetTouches));
            FastSetProperty("changedTouches", Engine.AsProperty(GetChangedTouches));
            FastSetProperty("altKey", Engine.AsProperty(GetAltKey));
            FastSetProperty("metaKey", Engine.AsProperty(GetMetaKey));
            FastSetProperty("ctrlKey", Engine.AsProperty(GetCtrlKey));
            FastSetProperty("shiftKey", Engine.AsProperty(GetShiftKey));
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

        public static TouchEventPrototype CreatePrototypeObject(EngineInstance engine, TouchEventConstructor constructor)
        {
            var obj = new TouchEventPrototype(engine)
            {
                Prototype = engine.Constructors.UIEvent.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InitUIEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
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
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetTouches(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.Touches);
        }


        JsValue GetTargetTouches(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.TargetTouches);
        }


        JsValue GetChangedTouches(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.ChangedTouches);
        }


        JsValue GetAltKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsAltPressed);
        }


        JsValue GetMetaKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsMetaPressed);
        }


        JsValue GetCtrlKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsCtrlPressed);
        }


        JsValue GetShiftKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsShiftPressed);
        }


        JsValue GetView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.View);
        }


        JsValue GetDetail(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.Detail);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchEventInstance>(Fail).RefTouchEvent;
            return _engine.GetDomNode(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TouchEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}