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

    sealed partial class TextPrototype : TextInstance
    {
        readonly EngineInstance _engine;

        public TextPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("splitText", Engine.AsValue(SplitText), true, true, true);
            FastSetProperty("wholeText", Engine.AsProperty(GetWholeText));
        }

        public static TextPrototype CreatePrototypeObject(EngineInstance engine, TextConstructor constructor)
        {
            var obj = new TextPrototype(engine)
            {
                Prototype = engine.Constructors.CharacterData.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue SplitText(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            var offset = TypeConverter.ToInt32(arguments.At(0));
            return _engine.GetDomNode(reference.Split(offset));
        }

        JsValue GetWholeText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            return _engine.GetDomNode(reference.Text);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Text]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}