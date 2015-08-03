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

    sealed partial class TextTrackCueListPrototype : TextTrackCueListInstance
    {
        public TextTrackCueListPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getCueById", Engine.AsValue(GetCueById), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static TextTrackCueListPrototype CreatePrototypeObject(EngineInstance engine, TextTrackCueListConstructor constructor)
        {
            var obj = new TextTrackCueListPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetCueById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextTrackCueListInstance>(Fail).RefTextTrackCueList;
            var id = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetCueById(id));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextTrackCueListInstance>(Fail).RefTextTrackCueList;
            return Engine.Select(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TextTrackCueList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}