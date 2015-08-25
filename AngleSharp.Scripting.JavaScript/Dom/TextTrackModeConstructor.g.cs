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

    sealed partial class TextTrackModeConstructor : FunctionInstance, IConstructor
    {
        readonly EngineInstance _engine;

        public TextTrackModeConstructor(EngineInstance engine)
            : base(engine.Jint, null, null, false)
        {
            _engine = engine;
            FastAddProperty("disabled", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Disabled), false, true, false);
            FastAddProperty("hidden", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Hidden), false, true, false);
            FastAddProperty("showing", (UInt32)(AngleSharp.Dom.Media.TextTrackMode.Showing), false, true, false);
        }

        public TextTrackModePrototype PrototypeObject 
        { 
            get; 
            private set; 
        }

        public static TextTrackModeConstructor CreateConstructor(EngineInstance engine)
        {
            var obj = new TextTrackModeConstructor(engine);
            obj.Extensible = true;
            obj.Prototype = engine.Jint.Function.PrototypeObject;
            obj.PrototypeObject = TextTrackModePrototype.CreatePrototypeObject(engine, obj);
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