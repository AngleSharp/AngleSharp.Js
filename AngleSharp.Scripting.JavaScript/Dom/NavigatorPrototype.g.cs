namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Navigator;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class NavigatorPrototype : NavigatorInstance
    {
        readonly EngineInstance _engine;

        public NavigatorPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("registerProtocolHandler", Engine.AsValue(RegisterProtocolHandler), true, true, true);
            FastAddProperty("registerContentHandler", Engine.AsValue(RegisterContentHandler), true, true, true);
            FastAddProperty("isProtocolHandlerRegistered", Engine.AsValue(IsProtocolHandlerRegistered), true, true, true);
            FastAddProperty("isContentHandlerRegistered", Engine.AsValue(IsContentHandlerRegistered), true, true, true);
            FastAddProperty("unregisterProtocolHandler", Engine.AsValue(UnregisterProtocolHandler), true, true, true);
            FastAddProperty("unregisterContentHandler", Engine.AsValue(UnregisterContentHandler), true, true, true);
            FastAddProperty("yieldForStorageUpdates", Engine.AsValue(YieldForStorageUpdates), true, true, true);
            FastSetProperty("appName", Engine.AsProperty(GetAppName));
            FastSetProperty("appVersion", Engine.AsProperty(GetAppVersion));
            FastSetProperty("platform", Engine.AsProperty(GetPlatform));
            FastSetProperty("userAgent", Engine.AsProperty(GetUserAgent));
            FastSetProperty("onLine", Engine.AsProperty(GetOnLine));
        }

        public static NavigatorPrototype CreatePrototypeObject(EngineInstance engine, NavigatorConstructor constructor)
        {
            var obj = new NavigatorPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue RegisterProtocolHandler(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var scheme = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            var title = TypeConverter.ToString(arguments.At(2));
            reference.RegisterProtocolHandler(scheme, url, title);
            return JsValue.Undefined;
        }

        JsValue RegisterContentHandler(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var mimeType = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            var title = TypeConverter.ToString(arguments.At(2));
            reference.RegisterContentHandler(mimeType, url, title);
            return JsValue.Undefined;
        }

        JsValue IsProtocolHandlerRegistered(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var scheme = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            return _engine.GetDomNode(reference.IsProtocolHandlerRegistered(scheme, url));
        }

        JsValue IsContentHandlerRegistered(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var mimeType = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            return _engine.GetDomNode(reference.IsContentHandlerRegistered(mimeType, url));
        }

        JsValue UnregisterProtocolHandler(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var scheme = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            reference.UnregisterProtocolHandler(scheme, url);
            return JsValue.Undefined;
        }

        JsValue UnregisterContentHandler(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            var mimeType = TypeConverter.ToString(arguments.At(0));
            var url = TypeConverter.ToString(arguments.At(1));
            reference.UnregisterContentHandler(mimeType, url);
            return JsValue.Undefined;
        }

        JsValue YieldForStorageUpdates(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            reference.WaitForStorageUpdates();
            return JsValue.Undefined;
        }

        JsValue GetAppName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            return _engine.GetDomNode(reference.Name);
        }


        JsValue GetAppVersion(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            return _engine.GetDomNode(reference.Version);
        }


        JsValue GetPlatform(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            return _engine.GetDomNode(reference.Platform);
        }


        JsValue GetUserAgent(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            return _engine.GetDomNode(reference.UserAgent);
        }


        JsValue GetOnLine(JsValue thisObj)
        {
            var reference = thisObj.TryCast<NavigatorInstance>(Fail).RefNavigator;
            return _engine.GetDomNode(reference.IsOnline);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Navigator]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}