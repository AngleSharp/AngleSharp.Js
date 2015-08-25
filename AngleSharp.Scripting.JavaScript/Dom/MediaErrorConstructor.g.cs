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

    sealed partial class MediaErrorConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public MediaErrorConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("MEDIA_ERR_ABORTED", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Aborted), false, true, false);
            FastAddProperty("MEDIA_ERR_NETWORK", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Network), false, true, false);
            FastAddProperty("MEDIA_ERR_DECODE", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.Decode), false, true, false);
            FastAddProperty("MEDIA_ERR_SRC_NOT_SUPPORTED", (UInt32)(AngleSharp.Dom.Media.MediaErrorCode.SourceNotSupported), false, true, false);
        }

        public MediaErrorPrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static MediaErrorConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new MediaErrorConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = MediaErrorPrototype.CreatePrototypeObject(engine, obj);
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