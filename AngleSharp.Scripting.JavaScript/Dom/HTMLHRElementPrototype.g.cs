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

    sealed partial class HTMLHRElementPrototype : HTMLHRElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLHRElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static HTMLHRElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLHRElementConstructor constructor)
        {
            var obj = new HTMLHRElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLHRElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}