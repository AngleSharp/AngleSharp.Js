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

    sealed partial class HTMLScriptElementPrototype : HTMLScriptElementInstance
    {
        public HTMLScriptElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("async", Engine.AsProperty(GetAsync, SetAsync));
            FastSetProperty("defer", Engine.AsProperty(GetDefer, SetDefer));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("charset", Engine.AsProperty(GetCharset, SetCharset));
            FastSetProperty("crossOrigin", Engine.AsProperty(GetCrossOrigin, SetCrossOrigin));
            FastSetProperty("text", Engine.AsProperty(GetText, SetText));
        }

        public static HTMLScriptElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLScriptElementConstructor constructor)
        {
            var obj = new HTMLScriptElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetAsync(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.IsAsync);
        }

        void SetAsync(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsAsync = value;
        }

        JsValue GetDefer(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.IsDeferred);
        }

        void SetDefer(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDeferred = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetCharset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.CharacterSet);
        }

        void SetCharset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.CharacterSet = value;
        }

        JsValue GetCrossOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.CrossOrigin);
        }

        void SetCrossOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.CrossOrigin = value;
        }

        JsValue GetText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.Text);
        }

        void SetText(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.Text = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLScriptElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}