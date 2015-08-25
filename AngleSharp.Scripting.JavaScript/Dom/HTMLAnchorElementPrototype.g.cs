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

    sealed partial class HTMLAnchorElementPrototype : HTMLAnchorElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLAnchorElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("target", Engine.AsProperty(GetTarget, SetTarget));
            FastSetProperty("download", Engine.AsProperty(GetDownload, SetDownload));
            FastSetProperty("ping", Engine.AsProperty(GetPing));
            FastSetProperty("rel", Engine.AsProperty(GetRel, SetRel));
            FastSetProperty("relList", Engine.AsProperty(GetRelList));
            FastSetProperty("hreflang", Engine.AsProperty(GetHreflang, SetHreflang));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("text", Engine.AsProperty(GetText));
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

        public static HTMLAnchorElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLAnchorElementConstructor constructor)
        {
            var obj = new HTMLAnchorElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Target);
        }

        void SetTarget(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Target = value;
        }

        JsValue GetDownload(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Download);
        }

        void SetDownload(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Download = value;
        }

        JsValue GetPing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Ping);
        }


        JsValue GetRel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Relation);
        }

        void SetRel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Relation = value;
        }

        JsValue GetRelList(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.RelationList);
        }


        JsValue GetHreflang(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.TargetLanguage);
        }

        void SetHreflang(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.TargetLanguage = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetText(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Text);
        }


        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Href);
        }

        void SetHref(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Href = value;
        }

        JsValue GetProtocol(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Protocol);
        }

        void SetProtocol(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Protocol = value;
        }

        JsValue GetHost(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Host);
        }

        void SetHost(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Host = value;
        }

        JsValue GetHostname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.HostName);
        }

        void SetHostname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.HostName = value;
        }

        JsValue GetPort(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Port);
        }

        void SetPort(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Port = value;
        }

        JsValue GetPathname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.PathName);
        }

        void SetPathname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.PathName = value;
        }

        JsValue GetSearch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Search);
        }

        void SetSearch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Search = value;
        }

        JsValue GetHash(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Hash);
        }

        void SetHash(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Hash = value;
        }

        JsValue GetUsername(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.UserName);
        }

        void SetUsername(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.UserName = value;
        }

        JsValue GetPassword(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Password);
        }

        void SetPassword(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            var value = TypeConverter.ToString(argument);
            reference.Password = value;
        }

        JsValue GetOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLAnchorElementInstance>(Fail).RefHTMLAnchorElement;
            return _engine.GetDomNode(reference.Origin);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLAnchorElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}