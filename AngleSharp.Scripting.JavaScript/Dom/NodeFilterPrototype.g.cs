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

    sealed partial class NodeFilterPrototype : NodeFilterInstance
    {
        readonly EngineInstance _engine;

        public NodeFilterPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static NodeFilterPrototype CreatePrototypeObject(EngineInstance engine, NodeFilterConstructor constructor)
        {
            var obj = new NodeFilterPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object NodeFilter]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}