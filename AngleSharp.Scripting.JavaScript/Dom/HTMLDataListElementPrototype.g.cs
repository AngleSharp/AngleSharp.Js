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

    sealed partial class HTMLDataListElementPrototype : HTMLDataListElementInstance
    {
        public HTMLDataListElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("options", Engine.AsProperty(GetOptions));
        }

        public static HTMLDataListElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLDataListElementConstructor constructor)
        {
            var obj = new HTMLDataListElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetOptions(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLDataListElementInstance>(Fail).RefHTMLDataListElement;
            return Engine.Select(reference.Options);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLDataListElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}