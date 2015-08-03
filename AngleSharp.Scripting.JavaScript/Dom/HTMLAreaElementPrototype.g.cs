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

    sealed partial class HTMLAreaElementPrototype : HTMLAreaElementInstance
    {
        public HTMLAreaElementPrototype(Engine engine)
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
            FastSetProperty("alt", Engine.AsProperty(GetAlt, SetAlt));
            FastSetProperty("coords", Engine.AsProperty(GetCoords, SetCoords));
            FastSetProperty("shape", Engine.AsProperty(GetShape, SetShape));
            FastSetProperty("target", Engine.AsProperty(GetTarget, SetTarget));
            FastSetProperty("download", Engine.AsProperty(GetDownload, SetDownload));
            FastSetProperty("ping", Engine.AsProperty(GetPing));
            FastSetProperty("rel", Engine.AsProperty(GetRel, SetRel));
            FastSetProperty("relList", Engine.AsProperty(GetRelList));
            FastSetProperty("hreflang", Engine.AsProperty(GetHreflang, SetHreflang));
            FastSetProperty("type", Engine.AsProperty(GetType, SetType));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
            FastSetProperty("href", Engine.AsProperty(GetHref, SetHref));
            FastSetProperty("protocol", Engine.AsProperty(GetProtocol, SetProtocol));
            FastSetProperty("host", Engine.AsProperty(GetHost, SetHost));
            FastSetProperty("hostname", Engine.AsProperty(GetHostname, SetHostname));
            FastSetProperty("port", Engine.AsProperty(GetPort, SetPort));
            FastSetProperty("pathname", Engine.AsProperty(GetPathname, SetPathname));
            FastSetProperty("search", Engine.AsProperty(GetSearch, SetSearch));
            FastSetProperty("hash", Engine.AsProperty(GetHash, SetHash));
            FastSetProperty("username", Engine.AsProperty(GetUsername, SetUsername));
            FastSetProperty("password", Engine.AsProperty(GetPassword, SetPassword));
            FastSetProperty("origin", Engine.AsProperty(GetOrigin));
        }

        public static HTMLAreaElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLAreaElementConstructor constructor)
        {
            var obj = new HTMLAreaElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetAlt(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.AlternativeText);
        }

        void SetAlt(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.AlternativeText = value;
        }

        JsValue GetCoords(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Coordinates);
        }

        void SetCoords(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Coordinates = value;
        }

        JsValue GetShape(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Shape);
        }

        void SetShape(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Shape = value;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Target);
        }

        void SetTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Target = value;
        }

        JsValue GetDownload(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Download);
        }

        void SetDownload(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Download = value;
        }

        JsValue GetPing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Ping);
        }


        JsValue GetRel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Relation);
        }

        void SetRel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Relation = value;
        }

        JsValue GetRelList(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.RelationList);
        }


        JsValue GetHreflang(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.TargetLanguage);
        }

        void SetHreflang(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.TargetLanguage = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Type);
        }

        void SetType(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Type = value;
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Href);
        }

        void SetHref(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Href = value;
        }

        JsValue GetProtocol(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Protocol);
        }

        void SetProtocol(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Protocol = value;
        }

        JsValue GetHost(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Host);
        }

        void SetHost(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Host = value;
        }

        JsValue GetHostname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.HostName);
        }

        void SetHostname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.HostName = value;
        }

        JsValue GetPort(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Port);
        }

        void SetPort(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Port = value;
        }

        JsValue GetPathname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.PathName);
        }

        void SetPathname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.PathName = value;
        }

        JsValue GetSearch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Search);
        }

        void SetSearch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Search = value;
        }

        JsValue GetHash(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Hash);
        }

        void SetHash(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Hash = value;
        }

        JsValue GetUsername(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.UserName);
        }

        void SetUsername(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.UserName = value;
        }

        JsValue GetPassword(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Password);
        }

        void SetPassword(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            var value = TypeConverter.ToString(argument);
            reference.Password = value;
        }

        JsValue GetOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAreaElementInstance>(Fail).RefHTMLAreaElement;
            return Engine.Select(reference.Origin);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLAreaElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}