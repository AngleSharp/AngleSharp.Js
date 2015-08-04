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

    sealed partial class CharacterDataPrototype : CharacterDataInstance
    {
        public CharacterDataPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("substringData", Engine.AsValue(SubstringData), true, true, true);
            FastAddProperty("appendData", Engine.AsValue(AppendData), true, true, true);
            FastAddProperty("insertData", Engine.AsValue(InsertData), true, true, true);
            FastAddProperty("deleteData", Engine.AsValue(DeleteData), true, true, true);
            FastAddProperty("replaceData", Engine.AsValue(ReplaceData), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("data", Engine.AsProperty(GetData, SetData));
            FastSetProperty("length", Engine.AsProperty(GetLength));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
        }

        public static CharacterDataPrototype CreatePrototypeObject(EngineInstance engine, CharacterDataConstructor constructor)
        {
            var obj = new CharacterDataPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Node.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue SubstringData(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var offset = TypeConverter.ToInt32(arguments.At(0));
            var count = TypeConverter.ToInt32(arguments.At(1));
            return Engine.Select(reference.Substring(offset, count));
        }

        JsValue AppendData(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var value = TypeConverter.ToString(arguments.At(0));
            reference.Append(value);
            return JsValue.Undefined;
        }

        JsValue InsertData(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var offset = TypeConverter.ToInt32(arguments.At(0));
            var value = TypeConverter.ToString(arguments.At(1));
            reference.Insert(offset, value);
            return JsValue.Undefined;
        }

        JsValue DeleteData(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var offset = TypeConverter.ToInt32(arguments.At(0));
            var count = TypeConverter.ToInt32(arguments.At(1));
            reference.Delete(offset, count);
            return JsValue.Undefined;
        }

        JsValue ReplaceData(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var offset = TypeConverter.ToInt32(arguments.At(0));
            var count = TypeConverter.ToInt32(arguments.At(1));
            var value = TypeConverter.ToString(arguments.At(2));
            reference.Replace(offset, count, value);
            return JsValue.Undefined;
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var nodes = new AngleSharp.Dom.INode[Math.Max(0, arguments.Length - 0)];

            for (var i = 0; i < nodes.Length; i++)
                nodes[i] = DomTypeConverter.ToNode(arguments.At(i + 0));

            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetData(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            return Engine.Select(reference.Data);
        }

        void SetData(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            var value = TypeConverter.ToString(argument);
            reference.Data = value;
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            return Engine.Select(reference.Length);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CharacterDataInstance>(Fail).RefCharacterData;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CharacterData]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}