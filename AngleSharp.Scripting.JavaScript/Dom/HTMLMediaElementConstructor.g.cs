namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class HTMLMediaElementConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public HTMLMediaElementConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("NETWORK_EMPTY", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Empty), false, true, false);
            FastAddProperty("NETWORK_IDLE", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Idle), false, true, false);
            FastAddProperty("NETWORK_LOADING", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.Loading), false, true, false);
            FastAddProperty("NETWORK_NO_SOURCE", (UInt32)(AngleSharp.Dom.Media.MediaNetworkState.NoSource), false, true, false);
            FastAddProperty("HAVE_NOTHING", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.Nothing), false, true, false);
            FastAddProperty("HAVE_METADATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.Metadata), false, true, false);
            FastAddProperty("HAVE_CURRENT_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.CurrentData), false, true, false);
            FastAddProperty("HAVE_FUTURE_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.FutureData), false, true, false);
            FastAddProperty("HAVE_ENOUGH_DATA", (UInt32)(AngleSharp.Dom.Media.MediaReadyState.EnoughData), false, true, false);
        }

        public HTMLMediaElementPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static HTMLMediaElementConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new HTMLMediaElementConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = HTMLMediaElementPrototype.CreatePrototypeObject(engine, obj);
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