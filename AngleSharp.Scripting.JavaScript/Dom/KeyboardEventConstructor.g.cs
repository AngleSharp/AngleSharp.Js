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

    sealed partial class KeyboardEventConstructor : FunctionInstance, IConstructor
    {
        public KeyboardEventConstructor(Engine engine)
            : base(engine, null, null, false)
        {
            FastAddProperty("DOM_KEY_LOCATION_STANDARD", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Standard), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_LEFT", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Left), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_RIGHT", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.Right), false, true, false);
            FastAddProperty("DOM_KEY_LOCATION_NUMPAD", (UInt32)(AngleSharp.Dom.Events.KeyboardLocation.NumPad), false, true, false);
        }

        public KeyboardEventPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static KeyboardEventConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new KeyboardEventConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = KeyboardEventPrototype.CreatePrototypeObject(engine, obj);
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
                var reference = new KeyboardEvent(type, eventInitDict);
                return new KeyboardEventInstance(Engine)
                {
                    Prototype = PrototypeObject,
                    RefKeyboardEvent = reference,
                    Extensible = true
                };
            }
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}