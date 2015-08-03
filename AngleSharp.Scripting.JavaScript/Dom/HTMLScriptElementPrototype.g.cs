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
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("before", Engine.AsValue(Before), true, true, true);
            FastAddProperty("after", Engine.AsValue(After), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("remove", Engine.AsValue(Remove), true, true, true);
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("async", Engine.AsProperty(GetAsync, SetAsync));
            FastSetProperty("defer", Engine.AsProperty(GetDefer, SetDefer));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("charset", Engine.AsProperty(GetCharset, SetCharset));
            FastSetProperty("crossOrigin", Engine.AsProperty(GetCrossOrigin, SetCrossOrigin));
            FastSetProperty("text", Engine.AsProperty(GetText, SetText));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
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

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            reference.Remove();
            return JsValue.Undefined;
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

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLScriptElementInstance>(Fail).RefHTMLScriptElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
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