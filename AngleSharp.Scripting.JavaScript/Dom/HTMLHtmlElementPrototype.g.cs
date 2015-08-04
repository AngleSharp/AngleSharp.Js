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

    sealed partial class HTMLHtmlElementPrototype : HTMLHtmlElementInstance
    {
        public HTMLHtmlElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("manifest", Engine.AsProperty(GetManifest, SetManifest));
        }

        public static HTMLHtmlElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLHtmlElementConstructor constructor)
        {
            var obj = new HTMLHtmlElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetManifest(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLHtmlElementInstance>(Fail).RefHTMLHtmlElement;
            return Engine.Select(reference.Manifest);
        }

        void SetManifest(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLHtmlElementInstance>(Fail).RefHTMLHtmlElement;
            var value = TypeConverter.ToString(argument);
            reference.Manifest = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLHtmlElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}