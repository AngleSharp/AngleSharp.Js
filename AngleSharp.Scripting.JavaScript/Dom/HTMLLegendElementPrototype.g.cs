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

    sealed partial class HTMLLegendElementPrototype : HTMLLegendElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLLegendElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("form", Engine.AsProperty(GetForm));
        }

        public static HTMLLegendElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLLegendElementConstructor constructor)
        {
            var obj = new HTMLLegendElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLegendElementInstance>(Fail).RefHTMLLegendElement;
            return _engine.GetDomNode(reference.Form);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLLegendElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}