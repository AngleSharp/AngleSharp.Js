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
        public TextPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("splitText", Engine.AsValue(SplitText), true, true, true);
            FastSetProperty("wholeText", Engine.AsProperty(GetWholeText));
        }

        public static TextPrototype CreatePrototypeObject(EngineInstance engine, TextConstructor constructor)
        {
            var obj = new TextPrototype(engine.Jint)
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
            return Engine.Select(reference.Split(offset));
        }

        JsValue GetWholeText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            return Engine.Select(reference.Text);
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