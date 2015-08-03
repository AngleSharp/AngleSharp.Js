namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class HTMLAudioElementPrototype : HTMLAudioElementInstance
    {
        public HTMLAudioElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static HTMLAudioElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLAudioElementConstructor constructor)
        {
            var obj = new HTMLAudioElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLMediaElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLAudioElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}