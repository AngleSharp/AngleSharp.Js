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

    sealed partial class HTMLDetailsElementPrototype : HTMLDetailsElementInstance
    {
        public HTMLDetailsElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("open", Engine.AsProperty(GetOpen, SetOpen));
        }

        public static HTMLDetailsElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLDetailsElementConstructor constructor)
        {
            var obj = new HTMLDetailsElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetOpen(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLDetailsElementInstance>(Fail).RefHTMLDetailsElement;
            return Engine.Select(reference.IsOpen);
        }

        void SetOpen(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLDetailsElementInstance>(Fail).RefHTMLDetailsElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsOpen = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLDetailsElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}