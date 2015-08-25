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

    sealed partial class ApplicationCacheConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public ApplicationCacheConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("UNCACHED", (UInt32)(AngleSharp.Dom.CacheStatus.Uncached), false, true, false);
            FastAddProperty("IDLE", (UInt32)(AngleSharp.Dom.CacheStatus.Idle), false, true, false);
            FastAddProperty("CHECKING", (UInt32)(AngleSharp.Dom.CacheStatus.Checking), false, true, false);
            FastAddProperty("DOWNLOADING", (UInt32)(AngleSharp.Dom.CacheStatus.Downloading), false, true, false);
            FastAddProperty("UPDATEREADY", (UInt32)(AngleSharp.Dom.CacheStatus.UpdateReady), false, true, false);
            FastAddProperty("OBSOLETE", (UInt32)(AngleSharp.Dom.CacheStatus.Obsolete), false, true, false);
        }

        public ApplicationCachePrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static ApplicationCacheConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new ApplicationCacheConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = ApplicationCachePrototype.CreatePrototypeObject(engine, obj);
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