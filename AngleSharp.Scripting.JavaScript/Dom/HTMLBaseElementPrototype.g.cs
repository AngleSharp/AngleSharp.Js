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

    sealed partial class HTMLBaseElementPrototype : HTMLBaseElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLBaseElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("href", Engine.AsProperty(GetHref, SetHref));
            FastSetProperty("Target", Engine.AsProperty(GetTarget, SetTarget));
        }

        public static HTMLBaseElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLBaseElementConstructor constructor)
        {
            var obj = new HTMLBaseElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBaseElementInstance>(Fail).RefHTMLBaseElement;
            return _engine.GetDomNode(reference.Href);
        }

        void SetHref(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLBaseElementInstance>(Fail).RefHTMLBaseElement;
            var value = TypeConverter.ToString(argument);
            reference.Href = value;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBaseElementInstance>(Fail).RefHTMLBaseElement;
            return _engine.GetDomNode(reference.Target);
        }

        void SetTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLBaseElementInstance>(Fail).RefHTMLBaseElement;
            var value = TypeConverter.ToString(argument);
            reference.Target = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLBaseElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}