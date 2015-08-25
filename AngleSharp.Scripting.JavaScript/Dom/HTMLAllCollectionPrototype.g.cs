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

    sealed partial class HTMLAllCollectionPrototype : HTMLAllCollectionInstance
    {
        readonly EngineInstance _engine;

        public HTMLAllCollectionPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static HTMLAllCollectionPrototype CreatePrototypeObject(EngineInstance engine, HTMLAllCollectionConstructor constructor)
        {
            var obj = new HTMLAllCollectionPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLCollection.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLAllCollection]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}