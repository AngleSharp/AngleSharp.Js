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

    sealed partial class MutationObserverPrototype : MutationObserverInstance
    {
        readonly EngineInstance _engine;

        public MutationObserverPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("disconnect", Engine.AsValue(Disconnect), true, true, true);
            FastAddProperty("observe", Engine.AsValue(Observe), true, true, true);
            FastAddProperty("takeRecords", Engine.AsValue(TakeRecords), true, true, true);
        }

        public static MutationObserverPrototype CreatePrototypeObject(EngineInstance engine, MutationObserverConstructor constructor)
        {
            var obj = new MutationObserverPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Disconnect(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MutationObserverInstance>(Fail).RefMutationObserver;
            reference.Disconnect();
            return JsValue.Undefined;
        }

        JsValue Observe(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MutationObserverInstance>(Fail).RefMutationObserver;
            var target = DomTypeConverter.ToNode(arguments.At(0));
            var options = SystemTypeConverter.ToObjBag(arguments.At(1));
            reference.Connect(target, options);
            return JsValue.Undefined;
        }

        JsValue TakeRecords(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MutationObserverInstance>(Fail).RefMutationObserver;
            return _engine.GetDomNode(reference.Flush());
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MutationObserver]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}