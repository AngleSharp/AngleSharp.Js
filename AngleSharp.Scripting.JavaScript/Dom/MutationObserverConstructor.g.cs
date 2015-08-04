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

    sealed partial class MutationObserverConstructor : FunctionInstance, IConstructor
    {
        public MutationObserverConstructor(Engine engine)
            : base(engine, null, null, false)
        {
        }

        public MutationObserverPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static MutationObserverConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new MutationObserverConstructor(engine.Jint);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = MutationObserverPrototype.CreatePrototypeObject(engine, obj);
            obj.FastAddProperty("length", 1, false, false, false);
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);
            return obj;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new JavaScriptException(Engine.TypeError);
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            if (arguments.Length == 1)
            {
                var callback = DomTypeConverter.ToMutationCallback(arguments.At(0));
                var reference = new MutationObserver(callback);
                return new MutationObserverInstance(Engine)
                {
                    Prototype = PrototypeObject,
                    RefMutationObserver = reference,
                    Extensible = true
                };
            }
            throw new JavaScriptException(Engine.TypeError);         
        }
    }
}