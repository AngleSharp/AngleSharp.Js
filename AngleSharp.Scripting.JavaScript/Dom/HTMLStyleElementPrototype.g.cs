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

    sealed partial class HTMLStyleElementPrototype : HTMLStyleElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLStyleElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("scoped", Engine.AsProperty(GetScoped, SetScoped));
            FastSetProperty("sheet", Engine.AsProperty(GetSheet));
        }

        public static HTMLStyleElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLStyleElementConstructor constructor)
        {
            var obj = new HTMLStyleElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return _engine.GetDomNode(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return _engine.GetDomNode(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToString(argument);
            reference.Media = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetScoped(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return _engine.GetDomNode(reference.IsScoped);
        }

        void SetScoped(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsScoped = value;
        }

        JsValue GetSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return _engine.GetDomNode(reference.Sheet);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLStyleElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}