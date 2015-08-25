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

    sealed partial class NodeListConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public NodeListConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
        }

        public NodeListPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static NodeListConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new NodeListConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = NodeListPrototype.CreatePrototypeObject(engine, obj);
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