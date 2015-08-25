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

    sealed partial class StyleSheetListPrototype : StyleSheetListInstance
    {
        readonly EngineInstance _engine;

        public StyleSheetListPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static StyleSheetListPrototype CreatePrototypeObject(EngineInstance engine, StyleSheetListConstructor constructor)
        {
            var obj = new StyleSheetListPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetListInstance>(Fail).RefStyleSheetList;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object StyleSheetList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}