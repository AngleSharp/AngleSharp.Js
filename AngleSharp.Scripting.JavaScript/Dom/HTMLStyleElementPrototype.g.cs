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
        public HTMLStyleElementPrototype(Engine engine)
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
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("scoped", Engine.AsProperty(GetScoped, SetScoped));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
            FastSetProperty("sheet", Engine.AsProperty(GetSheet));
        }

        public static HTMLStyleElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLStyleElementConstructor constructor)
        {
            var obj = new HTMLStyleElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.IsDisabled);
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
            return Engine.Select(reference.Media);
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
            return Engine.Select(reference.Type);
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
            return Engine.Select(reference.IsScoped);
        }

        void SetScoped(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsScoped = value;
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue GetSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLStyleElementInstance>(Fail).RefHTMLStyleElement;
            return Engine.Select(reference.Sheet);
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