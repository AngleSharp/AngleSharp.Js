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

    sealed partial class HTMLTemplateElementPrototype : HTMLTemplateElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTemplateElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("content", Engine.AsProperty(GetContent));
        }

        public static HTMLTemplateElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTemplateElementConstructor constructor)
        {
            var obj = new HTMLTemplateElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTemplateElementInstance>(Fail).RefHTMLTemplateElement;
            return _engine.GetDomNode(reference.Content);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTemplateElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}