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

    sealed partial class TrackEventConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public TrackEventConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
        }

        public TrackEventPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static TrackEventConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new TrackEventConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = TrackEventPrototype.CreatePrototypeObject(engine, obj);
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
                var reference = new TrackEvent(type, eventInitDict);
                return new TrackEventInstance(_engine)
                {
                    Prototype = PrototypeObject,
                    RefTrackEvent = reference,
                    Extensible = true
                };
            }
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}