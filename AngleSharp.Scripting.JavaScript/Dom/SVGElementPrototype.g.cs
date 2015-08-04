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

    sealed partial class SVGElementPrototype : SVGElementInstance
    {
        public SVGElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static SVGElementPrototype CreatePrototypeObject(EngineInstance engine, SVGElementConstructor constructor)
        {
            var obj = new SVGElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Element.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<SVGElementInstance>(Fail).RefSVGElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<SVGElementInstance>(Fail).RefSVGElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object SVGElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}