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

    sealed partial class HTMLTrackElementConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public HTMLTrackElementConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("NONE", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.None), false, true, false);
            FastAddProperty("LOADING", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Loading), false, true, false);
            FastAddProperty("LOADED", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Loaded), false, true, false);
            FastAddProperty("ERROR", (UInt32)(AngleSharp.Dom.Html.TrackReadyState.Error), false, true, false);
        }

        public HTMLTrackElementPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static HTMLTrackElementConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new HTMLTrackElementConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = HTMLTrackElementPrototype.CreatePrototypeObject(engine, obj);
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