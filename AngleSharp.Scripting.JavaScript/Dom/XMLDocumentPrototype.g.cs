namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Xml;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class XMLDocumentPrototype : XMLDocumentInstance
    {
        public XMLDocumentPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static XMLDocumentPrototype CreatePrototypeObject(EngineInstance engine, XMLDocumentConstructor constructor)
        {
            var obj = new XMLDocumentPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Document.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object XMLDocument]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}