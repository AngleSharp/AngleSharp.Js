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

    sealed partial class HTMLObjectElementPrototype : HTMLObjectElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLObjectElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("data", Engine.AsProperty(GetData, SetData));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("typeMustMatch", Engine.AsProperty(GetTypeMustMatch, SetTypeMustMatch));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("useMap", Engine.AsProperty(GetUseMap, SetUseMap));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("contentDocument", Engine.AsProperty(GetContentDocument));
            FastSetProperty("contentWindow", Engine.AsProperty(GetContentWindow));
        }

        public static HTMLObjectElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLObjectElementConstructor constructor)
        {
            var obj = new HTMLObjectElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetData(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.Source);
        }

        void SetData(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetTypeMustMatch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.TypeMustMatch);
        }

        void SetTypeMustMatch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.TypeMustMatch = value;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetUseMap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.UseMap);
        }

        void SetUseMap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToString(argument);
            reference.UseMap = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.Form);
        }


        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayHeight = value;
        }

        JsValue GetContentDocument(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.ContentDocument);
        }


        JsValue GetContentWindow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLObjectElementInstance>(Fail).RefHTMLObjectElement;
            return _engine.GetDomNode(reference.ContentWindow);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLObjectElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}