namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Css;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MediaListPrototype : MediaListInstance
    {
        readonly EngineInstance _engine;

        public MediaListPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("appendMedium", Engine.AsValue(AppendMedium), true, true, true);
            FastAddProperty("removeMedium", Engine.AsValue(RemoveMedium), true, true, true);
            FastSetProperty("mediaText", Engine.AsProperty(GetMediaText, SetMediaText));
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static MediaListPrototype CreatePrototypeObject(EngineInstance engine, MediaListConstructor constructor)
        {
            var obj = new MediaListPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue AppendMedium(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MediaListInstance>(Fail).RefMediaList;
            var medium = TypeConverter.ToString(arguments.At(0));
            reference.Add(medium);
            return JsValue.Undefined;
        }

        JsValue RemoveMedium(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MediaListInstance>(Fail).RefMediaList;
            var medium = TypeConverter.ToString(arguments.At(0));
            reference.Remove(medium);
            return JsValue.Undefined;
        }

        JsValue GetMediaText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaListInstance>(Fail).RefMediaList;
            return _engine.GetDomNode(reference.MediaText);
        }

        void SetMediaText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<MediaListInstance>(Fail).RefMediaList;
            var value = TypeConverter.ToString(argument);
            reference.MediaText = value;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<MediaListInstance>(Fail).RefMediaList;
            return _engine.GetDomNode(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MediaList]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}