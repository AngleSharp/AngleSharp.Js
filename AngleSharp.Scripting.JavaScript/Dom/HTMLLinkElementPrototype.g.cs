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

    sealed partial class HTMLLinkElementPrototype : HTMLLinkElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLLinkElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("href", Engine.AsProperty(GetHref, SetHref));
            FastSetProperty("rel", Engine.AsProperty(GetRel, SetRel));
            FastSetProperty("relList", Engine.AsProperty(GetRelList));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
            FastSetProperty("hreflang", Engine.AsProperty(GetHreflang, SetHreflang));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("sizes", Engine.AsProperty(GetSizes));
            FastSetProperty("sheet", Engine.AsProperty(GetSheet));
        }

        public static HTMLLinkElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLLinkElementConstructor constructor)
        {
            var obj = new HTMLLinkElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Href);
        }

        void SetHref(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToString(argument);
            reference.Href = value;
        }

        JsValue GetRel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Relation);
        }

        void SetRel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToString(argument);
            reference.Relation = value;
        }

        JsValue GetRelList(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.RelationList);
        }


        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToString(argument);
            reference.Media = value;
        }

        JsValue GetHreflang(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.TargetLanguage);
        }

        void SetHreflang(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToString(argument);
            reference.TargetLanguage = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetSizes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Sizes);
        }


        JsValue GetSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLLinkElementInstance>(Fail).RefHTMLLinkElement;
            return _engine.GetDomNode(reference.Sheet);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLLinkElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}