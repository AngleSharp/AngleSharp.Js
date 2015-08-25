namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class RangeConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public RangeConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("START_TO_START", (UInt32)(AngleSharp.Dom.RangeType.StartToStart), false, true, false);
            FastAddProperty("START_TO_END", (UInt32)(AngleSharp.Dom.RangeType.StartToEnd), false, true, false);
            FastAddProperty("END_TO_END", (UInt32)(AngleSharp.Dom.RangeType.EndToEnd), false, true, false);
            FastAddProperty("END_TO_START", (UInt32)(AngleSharp.Dom.RangeType.EndToStart), false, true, false);
        }

        public RangePrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static RangeConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new RangeConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = RangePrototype.CreatePrototypeObject(engine, obj);
            obj.FastAddProperty("length", 0, false, false, false);
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);
            return obj;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}