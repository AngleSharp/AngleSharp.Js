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

    sealed partial class SVGDocumentPrototype : SVGDocumentInstance
    {
        readonly EngineInstance _engine;

        public SVGDocumentPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("rootElement", Engine.AsProperty(GetRootElement));
        }

        public static SVGDocumentPrototype CreatePrototypeObject(EngineInstance engine, SVGDocumentConstructor constructor)
        {
            var obj = new SVGDocumentPrototype(engine)
            {
                Prototype = engine.Constructors.Document.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetRootElement(JsValue thisObj)
        {
            var reference = thisObj.TryCast<SVGDocumentInstance>(Fail).RefSVGDocument;
            return _engine.GetDomNode(reference.RootElement);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object SVGDocument]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}