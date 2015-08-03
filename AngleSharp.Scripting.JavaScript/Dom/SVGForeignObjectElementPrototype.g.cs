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

    sealed partial class SVGForeignObjectElementPrototype : SVGForeignObjectElementInstance
    {
        public SVGForeignObjectElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static SVGForeignObjectElementPrototype CreatePrototypeObject(EngineInstance engine, SVGForeignObjectElementConstructor constructor)
        {
            var obj = new SVGForeignObjectElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.SVGElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object SVGForeignObjectElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}