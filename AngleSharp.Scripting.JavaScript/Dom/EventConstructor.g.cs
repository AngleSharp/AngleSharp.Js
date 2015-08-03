namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class EventConstructor : FunctionInstance, IConstructor
    {
        public EventConstructor(Engine engine)
            : base(engine, null, null, false)
        {
            FastAddProperty("NONE", (UInt32)(AngleSharp.Dom.Events.EventPhase.None), false, true, false);
            FastAddProperty("CAPTURING_PHASE", (UInt32)(AngleSharp.Dom.Events.EventPhase.Capturing), false, true, false);
            FastAddProperty("AT_TARGET", (UInt32)(AngleSharp.Dom.Events.EventPhase.AtTarget), false, true, false);
            FastAddProperty("BUBBLING_PHASE", (UInt32)(AngleSharp.Dom.Events.EventPhase.Bubbling), false, true, false);
        }

        public EventPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static EventConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new EventConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = EventPrototype.CreatePrototypeObject(engine, obj);
            obj.FastAddProperty("length", 2, false, false, false);
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);
            return obj;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            if (arguments.Length == 2)
            {
                var type = TypeConverter.ToString(arguments.At(0));
                var eventInitDict = SystemTypeConverter.ToObjBag(arguments.At(1));
                var reference = new Event(type, eventInitDict);
                return new EventInstance(Engine)
                {
                    Prototype = PrototypeObject,
                    RefEvent = reference,
                    Extensible = true
                };
            }
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}