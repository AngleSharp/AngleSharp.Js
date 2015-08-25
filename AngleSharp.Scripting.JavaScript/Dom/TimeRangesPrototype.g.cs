namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class TimeRangesPrototype : TimeRangesInstance
    {
        readonly EngineInstance _engine;

        public TimeRangesPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("start", Engine.AsValue(Start), true, true, true);
            FastAddProperty("end", Engine.AsValue(End), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static TimeRangesPrototype CreatePrototypeObject(EngineInstance engine, TimeRangesConstructor constructor)
        {
            var obj = new TimeRangesPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Start(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TimeRangesInstance>(Fail).RefTimeRanges;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return _engine.GetDomNode(reference.Start(index));
        }

        JsValue End(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TimeRangesInstance>(Fail).RefTimeRanges;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return _engine.GetDomNode(reference.End(index));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TimeRangesInstance>(Fail).RefTimeRanges;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TimeRanges]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}