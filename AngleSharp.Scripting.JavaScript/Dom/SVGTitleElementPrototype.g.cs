namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Svg;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class SVGTitleElementPrototype : SVGTitleElementInstance
    {
        readonly EngineInstance _engine;

        public SVGTitleElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static SVGTitleElementPrototype CreatePrototypeObject(EngineInstance engine, SVGTitleElementConstructor constructor)
        {
            var obj = new SVGTitleElementPrototype(engine)
            {
                Prototype = engine.Constructors.SVGElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object SVGTitleElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}