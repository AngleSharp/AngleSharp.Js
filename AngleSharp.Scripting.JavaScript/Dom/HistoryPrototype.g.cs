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

    sealed partial class HistoryPrototype : HistoryInstance
    {
        readonly EngineInstance _engine;

        public HistoryPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("go", Engine.AsValue(Go), true, true, true);
            FastAddProperty("back", Engine.AsValue(Back), true, true, true);
            FastAddProperty("forward", Engine.AsValue(Forward), true, true, true);
            FastAddProperty("pushState", Engine.AsValue(PushState), true, true, true);
            FastAddProperty("replaceState", Engine.AsValue(ReplaceState), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("state", Engine.AsProperty(GetState));
        }

        public static HistoryPrototype CreatePrototypeObject(EngineInstance engine, HistoryConstructor constructor)
        {
            var obj = new HistoryPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Go(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            var delta = TypeConverter.ToInt32(arguments.At(0));
            reference.Go(delta);
            return JsValue.Undefined;
        }

        JsValue Back(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            reference.Back();
            return JsValue.Undefined;
        }

        JsValue Forward(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            reference.Forward();
            return JsValue.Undefined;
        }

        JsValue PushState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            var data = SystemTypeConverter.ToObject(arguments.At(0));
            var title = TypeConverter.ToString(arguments.At(1));
            var url = TypeConverter.ToString(arguments.At(2));
            reference.PushState(data, title, url);
            return JsValue.Undefined;
        }

        JsValue ReplaceState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            var data = SystemTypeConverter.ToObject(arguments.At(0));
            var title = TypeConverter.ToString(arguments.At(1));
            var url = TypeConverter.ToString(arguments.At(2));
            reference.ReplaceState(data, title, url);
            return JsValue.Undefined;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue GetState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HistoryInstance>(Fail).RefHistory;
            return _engine.GetDomNode(reference.State);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object History]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}