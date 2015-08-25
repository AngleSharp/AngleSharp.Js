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

    sealed partial class HTMLTitleElementPrototype : HTMLTitleElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTitleElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("text", Engine.AsProperty(GetText, SetText));
        }

        public static HTMLTitleElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTitleElementConstructor constructor)
        {
            var obj = new HTMLTitleElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTitleElementInstance>(Fail).RefHTMLTitleElement;
            return _engine.GetDomNode(reference.Text);
        }

        void SetText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTitleElementInstance>(Fail).RefHTMLTitleElement;
            var value = TypeConverter.ToString(argument);
            reference.Text = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTitleElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}