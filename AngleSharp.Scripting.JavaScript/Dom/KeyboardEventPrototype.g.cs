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

    sealed partial class KeyboardEventPrototype : KeyboardEventInstance
    {
        public KeyboardEventPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getModifierState", Engine.AsValue(GetModifierState), true, true, true);
            FastAddProperty("initKeyboardEvent", Engine.AsValue(InitKeyboardEvent), true, true, true);
            FastAddProperty("initUIEvent", Engine.AsValue(InitUIEvent), true, true, true);
            FastAddProperty("stopPropagation", Engine.AsValue(StopPropagation), true, true, true);
            FastAddProperty("stopImmediatePropagation", Engine.AsValue(StopImmediatePropagation), true, true, true);
            FastAddProperty("preventDefault", Engine.AsValue(PreventDefault), true, true, true);
            FastAddProperty("initEvent", Engine.AsValue(InitEvent), true, true, true);
            FastSetProperty("key", Engine.AsProperty(GetKey));
            FastSetProperty("location", Engine.AsProperty(GetLocation));
            FastSetProperty("ctrlKey", Engine.AsProperty(GetCtrlKey));
            FastSetProperty("shiftKey", Engine.AsProperty(GetShiftKey));
            FastSetProperty("altKey", Engine.AsProperty(GetAltKey));
            FastSetProperty("metaKey", Engine.AsProperty(GetMetaKey));
            FastSetProperty("repeat", Engine.AsProperty(GetRepeat));
            FastSetProperty("locale", Engine.AsProperty(GetLocale));
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

        public static KeyboardEventPrototype CreatePrototypeObject(EngineInstance engine, KeyboardEventConstructor constructor)
        {
            var obj = new KeyboardEventPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.UIEvent.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetModifierState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            var key = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetModifierState(key));
        }

        JsValue InitKeyboardEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            var view = DomTypeConverter.ToWindow(arguments.At(3));
            var detail = TypeConverter.ToInt32(arguments.At(4));
            var key = TypeConverter.ToString(arguments.At(5));
            var location = DomTypeConverter.ToKeyboardLocation(arguments.At(6));
            var modifiersList = TypeConverter.ToString(arguments.At(7));
            var repeat = TypeConverter.ToBoolean(arguments.At(8));
            reference.Init(type, bubbles, cancelable, view, detail, key, location, modifiersList, repeat);
            return JsValue.Undefined;
        }

        JsValue InitUIEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
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
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            reference.Stop();
            return JsValue.Undefined;
        }

        JsValue StopImmediatePropagation(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            reference.StopImmediately();
            return JsValue.Undefined;
        }

        JsValue PreventDefault(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            reference.Cancel();
            return JsValue.Undefined;
        }

        JsValue InitEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            var type = TypeConverter.ToString(arguments.At(0));
            var bubbles = TypeConverter.ToBoolean(arguments.At(1));
            var cancelable = TypeConverter.ToBoolean(arguments.At(2));
            reference.Init(type, bubbles, cancelable);
            return JsValue.Undefined;
        }

        JsValue GetKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Key);
        }


        JsValue GetLocation(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Location);
        }


        JsValue GetCtrlKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsCtrlPressed);
        }


        JsValue GetShiftKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsShiftPressed);
        }


        JsValue GetAltKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsAltPressed);
        }


        JsValue GetMetaKey(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsMetaPressed);
        }


        JsValue GetRepeat(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsRepeated);
        }


        JsValue GetLocale(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Locale);
        }


        JsValue GetView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.View);
        }


        JsValue GetDetail(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Detail);
        }


        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Type);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.OriginalTarget);
        }


        JsValue GetCurrentTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.CurrentTarget);
        }


        JsValue GetEventPhase(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Phase);
        }


        JsValue GetBubbles(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsBubbling);
        }


        JsValue GetCancelable(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsCancelable);
        }


        JsValue GetDefaultPrevented(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsDefaultPrevented);
        }


        JsValue GetIsTrusted(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.IsTrusted);
        }

        void SetIsTrusted(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsTrusted = value;
        }

        JsValue GetTimeStamp(JsValue thisObj)
        {
            var reference = thisObj.TryCast<KeyboardEventInstance>(Fail).RefKeyboardEvent;
            return Engine.Select(reference.Time);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object KeyboardEvent]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}