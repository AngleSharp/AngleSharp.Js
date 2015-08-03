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

    sealed partial class WheelEventConstructor : FunctionInstance, IConstructor
    {
        public WheelEventConstructor(Engine engine)
            : base(engine, null, null, false)
        {
            FastAddProperty("DOM_DELTA_PIXEL", (UInt32)(AngleSharp.Dom.Events.WheelMode.Pixel), false, true, false);
            FastAddProperty("DOM_DELTA_LINE", (UInt32)(AngleSharp.Dom.Events.WheelMode.Line), false, true, false);
            FastAddProperty("DOM_DELTA_PAGE", (UInt32)(AngleSharp.Dom.Events.WheelMode.Page), false, true, false);
        }

        public WheelEventPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static WheelEventConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new WheelEventConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = WheelEventPrototype.CreatePrototypeObject(engine, obj);
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
                var reference = new WheelEvent(type, eventInitDict);
                return new WheelEventInstance(Engine)
                {
                    Prototype = PrototypeObject,
                    RefWheelEvent = reference,
                    Extensible = true
                };
            }
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}