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

    sealed partial class HTMLMetaElementPrototype : HTMLMetaElementInstance
    {
        public HTMLMetaElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("httpEquiv", Engine.AsProperty(GetHttpEquiv, SetHttpEquiv));
            FastSetProperty("content", Engine.AsProperty(GetContent, SetContent));
        }

        public static HTMLMetaElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMetaElementConstructor constructor)
        {
            var obj = new HTMLMetaElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            return Engine.Select(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetHttpEquiv(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            return Engine.Select(reference.HttpEquivalent);
        }

        void SetHttpEquiv(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            var value = TypeConverter.ToString(argument);
            reference.HttpEquivalent = value;
        }

        JsValue GetContent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            return Engine.Select(reference.Content);
        }

        void SetContent(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMetaElementInstance>(Fail).RefHTMLMetaElement;
            var value = TypeConverter.ToString(argument);
            reference.Content = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLMetaElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}