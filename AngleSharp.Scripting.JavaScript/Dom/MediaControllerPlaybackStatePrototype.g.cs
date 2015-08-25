namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MediaControllerPlaybackStatePrototype : MediaControllerPlaybackStateInstance
    {
        readonly EngineInstance _engine;

        public MediaControllerPlaybackStatePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static MediaControllerPlaybackStatePrototype CreatePrototypeObject(EngineInstance engine, MediaControllerPlaybackStateConstructor constructor)
        {
            var obj = new MediaControllerPlaybackStatePrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MediaControllerPlaybackState]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}