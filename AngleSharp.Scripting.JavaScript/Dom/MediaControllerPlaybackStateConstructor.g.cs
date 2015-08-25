namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MediaControllerPlaybackStateConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public MediaControllerPlaybackStateConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("waiting", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Waiting), false, true, false);
            FastAddProperty("playing", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Playing), false, true, false);
            FastAddProperty("ended", (UInt32)(AngleSharp.Dom.Media.MediaControllerPlaybackState.Ended), false, true, false);
        }

        public MediaControllerPlaybackStatePrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static MediaControllerPlaybackStateConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new MediaControllerPlaybackStateConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = MediaControllerPlaybackStatePrototype.CreatePrototypeObject(engine, obj);
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