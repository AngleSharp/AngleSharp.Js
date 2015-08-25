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

    sealed partial class HTMLMenuElementPrototype : HTMLMenuElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMenuElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
        }

        public static HTMLMenuElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMenuElementConstructor constructor)
        {
            var obj = new HTMLMenuElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuElementInstance>(Fail).RefHTMLMenuElement;
            return _engine.GetDomNode(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuElementInstance>(Fail).RefHTMLMenuElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMenuElementInstance>(Fail).RefHTMLMenuElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMenuElementInstance>(Fail).RefHTMLMenuElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLMenuElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}