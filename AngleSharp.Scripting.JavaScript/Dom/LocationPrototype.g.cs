namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class LocationPrototype : LocationInstance
    {
        readonly EngineInstance _engine;

        public LocationPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("assign", Engine.AsValue(Assign), true, true, true);
            FastAddProperty("replace", Engine.AsValue(Replace), true, true, true);
            FastAddProperty("reload", Engine.AsValue(Reload), true, true, true);
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

        public static LocationPrototype CreatePrototypeObject(EngineInstance engine, LocationConstructor constructor)
        {
            var obj = new LocationPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Assign(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var url = TypeConverter.ToString(arguments.At(0));
            reference.Assign(url);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var url = TypeConverter.ToString(arguments.At(0));
            reference.Replace(url);
            return JsValue.Undefined;
        }

        JsValue Reload(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            reference.Reload();
            return JsValue.Undefined;
        }

        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Href);
        }

        void SetHref(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Href = value;
        }

        JsValue GetProtocol(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Protocol);
        }

        void SetProtocol(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Protocol = value;
        }

        JsValue GetHost(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Host);
        }

        void SetHost(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Host = value;
        }

        JsValue GetHostname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.HostName);
        }

        void SetHostname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.HostName = value;
        }

        JsValue GetPort(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Port);
        }

        void SetPort(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Port = value;
        }

        JsValue GetPathname(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.PathName);
        }

        void SetPathname(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.PathName = value;
        }

        JsValue GetSearch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Search);
        }

        void SetSearch(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Search = value;
        }

        JsValue GetHash(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Hash);
        }

        void SetHash(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Hash = value;
        }

        JsValue GetUsername(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.UserName);
        }

        void SetUsername(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.UserName = value;
        }

        JsValue GetPassword(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Password);
        }

        void SetPassword(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            var value = TypeConverter.ToString(argument);
            reference.Password = value;
        }

        JsValue GetOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<LocationInstance>(Fail).RefLocation;
            return _engine.GetDomNode(reference.Origin);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Location]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}