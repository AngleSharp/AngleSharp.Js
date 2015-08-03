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

    sealed partial class NamedNodeMapPrototype : NamedNodeMapInstance
    {
        public NamedNodeMapPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("getNamedItem", Engine.AsValue(GetNamedItem), true, true, true);
            FastAddProperty("setNamedItem", Engine.AsValue(SetNamedItem), true, true, true);
            FastAddProperty("removeNamedItem", Engine.AsValue(RemoveNamedItem), true, true, true);
            FastAddProperty("getNamedItemNS", Engine.AsValue(GetNamedItemNS), true, true, true);
            FastAddProperty("setNamedItemNS", Engine.AsValue(SetNamedItemNS), true, true, true);
            FastAddProperty("removeNamedItemNS", Engine.AsValue(RemoveNamedItemNS), true, true, true);
            FastSetProperty("length", Engine.AsProperty(GetLength));
        }

        public static NamedNodeMapPrototype CreatePrototypeObject(EngineInstance engine, NamedNodeMapConstructor constructor)
        {
            var obj = new NamedNodeMapPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetNamedItem(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetNamedItem(name));
        }

        JsValue SetNamedItem(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var item = DomTypeConverter.ToAttr(arguments.At(0));
            return Engine.Select(reference.SetNamedItem(item));
        }

        JsValue RemoveNamedItem(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.RemoveNamedItem(name));
        }

        JsValue GetNamedItemNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var localName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.GetNamedItem(namespaceUri, localName));
        }

        JsValue SetNamedItemNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var item = DomTypeConverter.ToAttr(arguments.At(0));
            return Engine.Select(reference.SetNamedItemWithNamespaceUri(item));
        }

        JsValue RemoveNamedItemNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var localName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.RemoveNamedItem(namespaceUri, localName));
        }

        JsValue GetLength(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NamedNodeMapInstance>(Fail).RefNamedNodeMap;
            return Engine.Select(reference.Length);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object NamedNodeMap]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}