namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class DOMImplementationPrototype : DOMImplementationInstance
    {
        readonly EngineInstance _engine;

        public DOMImplementationPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("createDocument", Engine.AsValue(CreateDocument), true, true, true);
            FastAddProperty("createHTMLDocument", Engine.AsValue(CreateHTMLDocument), true, true, true);
            FastAddProperty("createDocumentType", Engine.AsValue(CreateDocumentType), true, true, true);
            FastAddProperty("hasFeature", Engine.AsValue(HasFeature), true, true, true);
        }

        public static DOMImplementationPrototype CreatePrototypeObject(EngineInstance engine, DOMImplementationConstructor constructor)
        {
            var obj = new DOMImplementationPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue CreateDocument(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMImplementationInstance>(Fail).RefDOMImplementation;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var qualifiedName = TypeConverter.ToString(arguments.At(1));
            var doctype = DomTypeConverter.ToDoctype(arguments.At(2));
            return _engine.GetDomNode(reference.CreateDocument(namespaceUri, qualifiedName, doctype));
        }

        JsValue CreateHTMLDocument(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMImplementationInstance>(Fail).RefDOMImplementation;
            var title = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.CreateHtmlDocument(title));
        }

        JsValue CreateDocumentType(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMImplementationInstance>(Fail).RefDOMImplementation;
            var qualifiedName = TypeConverter.ToString(arguments.At(0));
            var publicId = TypeConverter.ToString(arguments.At(1));
            var systemId = TypeConverter.ToString(arguments.At(2));
            return _engine.GetDomNode(reference.CreateDocumentType(qualifiedName, publicId, systemId));
        }

        JsValue HasFeature(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMImplementationInstance>(Fail).RefDOMImplementation;
            var feature = TypeConverter.ToString(arguments.At(0));
            var version = TypeConverter.ToString(arguments.At(1));
            return _engine.GetDomNode(reference.HasFeature(feature, version));
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DOMImplementation]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}