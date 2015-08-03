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
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("wholeText", Engine.AsProperty(GetWholeText));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
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

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetWholeText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            return Engine.Select(reference.Text);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TextInstance>(Fail).RefText;
            return Engine.Select(reference.PreviousElementSibling);
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