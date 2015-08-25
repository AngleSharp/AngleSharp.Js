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

    sealed partial class HTMLCollectionPrototype : HTMLCollectionInstance
    {
        readonly EngineInstance _engine;

        public HTMLCollectionPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static HTMLCollectionPrototype CreatePrototypeObject(EngineInstance engine, HTMLCollectionConstructor constructor)
        {
            var obj = new HTMLCollectionPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCollectionInstance>(Fail).RefHTMLCollection;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLCollection]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}