namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class EventTargetPrototype : EventTargetInstance
    {
        public EventTargetPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("addEventListener", Engine.AsValue(AddEventListener), true, true, true);
            FastAddProperty("removeEventListener", Engine.AsValue(RemoveEventListener), true, true, true);
            FastAddProperty("dispatchEvent", Engine.AsValue(DispatchEvent), true, true, true);
        }

        public static EventTargetPrototype CreatePrototypeObject(EngineInstance engine, EventTargetConstructor constructor)
        {
            var obj = new EventTargetPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue AddEventListener(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<EventTargetInstance>(Fail).RefEventTarget;
            var type = TypeConverter.ToString(arguments.At(0));
            var callback = DomTypeConverter.ToEventHandler(arguments.At(1));
            var capture = TypeConverter.ToBoolean(arguments.At(2));
            reference.AddEventListener(type, callback, capture);
            return JsValue.Undefined;
        }

        JsValue RemoveEventListener(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<EventTargetInstance>(Fail).RefEventTarget;
            var type = TypeConverter.ToString(arguments.At(0));
            var callback = DomTypeConverter.ToEventHandler(arguments.At(1));
            var capture = TypeConverter.ToBoolean(arguments.At(2));
            reference.RemoveEventListener(type, callback, capture);
            return JsValue.Undefined;
        }

        JsValue DispatchEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<EventTargetInstance>(Fail).RefEventTarget;
            var ev = DomTypeConverter.ToEvent(arguments.At(0));
            return Engine.Select(reference.Dispatch(ev));
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object EventTarget]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}