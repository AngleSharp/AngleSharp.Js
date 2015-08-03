namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class HTMLOptionsCollectionPrototype : HTMLOptionsCollectionInstance
    {
        public HTMLOptionsCollectionPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("add", Engine.AsValue(Add), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("selectedIndex", Engine.AsProperty(GetSelectedIndex, SetSelectedIndex));
        }

        public static HTMLOptionsCollectionPrototype CreatePrototypeObject(EngineInstance engine, HTMLOptionsCollectionConstructor constructor)
        {
            var obj = new HTMLOptionsCollectionPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLCollection.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Add(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLOptionsCollectionInstance>(Fail).RefHTMLOptionsCollection;
            var element = DomTypeConverter.ToOptionsGroupElement(arguments.At(0));
            var before = DomTypeConverter.ToHtmlElement(arguments.At(1));
            reference.Add(element, before);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLOptionsCollectionInstance>(Fail).RefHTMLOptionsCollection;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.Remove(index);
            return JsValue.Undefined;
        }

        JsValue GetSelectedIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLOptionsCollectionInstance>(Fail).RefHTMLOptionsCollection;
            return Engine.Select(reference.SelectedIndex);
        }

        void SetSelectedIndex(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLOptionsCollectionInstance>(Fail).RefHTMLOptionsCollection;
            var value = TypeConverter.ToInt32(argument);
            reference.SelectedIndex = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLOptionsCollection]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}