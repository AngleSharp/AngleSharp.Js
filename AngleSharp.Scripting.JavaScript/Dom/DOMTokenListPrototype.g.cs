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

    sealed partial class DOMTokenListPrototype : DOMTokenListInstance
    {
        readonly EngineInstance _engine;

        public DOMTokenListPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("contains", Engine.AsValue(Contains), true, true, true);
            FastAddProperty("add", Engine.AsValue(Add), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastAddProperty("toggle", Engine.AsValue(Toggle), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static DOMTokenListPrototype CreatePrototypeObject(EngineInstance engine, DOMTokenListConstructor constructor)
        {
            var obj = new DOMTokenListPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Contains(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMTokenListInstance>(Fail).RefDOMTokenList;
            var token = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.Contains(token));
        }

        JsValue Add(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMTokenListInstance>(Fail).RefDOMTokenList;
            var tokens = new System.String[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < tokens.Length; i++)
                tokens[i] = TypeConverter.ToString(arguments.At(i + 0));

            reference.Add(tokens);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMTokenListInstance>(Fail).RefDOMTokenList;
            var tokens = new System.String[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < tokens.Length; i++)
                tokens[i] = TypeConverter.ToString(arguments.At(i + 0));

            reference.Remove(tokens);
            return JsValue.Undefined;
        }

        JsValue Toggle(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DOMTokenListInstance>(Fail).RefDOMTokenList;
            var token = TypeConverter.ToString(arguments.At(0));
            var force = TypeConverter.ToBoolean(arguments.At(1));
            return _engine.GetDomNode(reference.Toggle(token, force));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DOMTokenListInstance>(Fail).RefDOMTokenList;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object DOMTokenList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}