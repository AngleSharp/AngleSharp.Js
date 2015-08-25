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

    sealed partial class ApplicationCachePrototype : ApplicationCacheInstance
    {
        readonly EngineInstance _engine;

        public ApplicationCachePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("update", Engine.AsValue(Update), true, true, true);
            FastAddProperty("abort", Engine.AsValue(Abort), true, true, true);
            FastAddProperty("swapCache", Engine.AsValue(SwapCache), true, true, true);
            FastSetProperty("status", Engine.AsProperty(GetStatus));
            FastSetProperty("onchecking", Engine.AsProperty(GetCheckingEvent, SetCheckingEvent));
            FastSetProperty("onerror", Engine.AsProperty(GetErrorEvent, SetErrorEvent));
            FastSetProperty("onnoupdate", Engine.AsProperty(GetNoUpdateEvent, SetNoUpdateEvent));
            FastSetProperty("ondownloading", Engine.AsProperty(GetDownloadingEvent, SetDownloadingEvent));
            FastSetProperty("onprogress", Engine.AsProperty(GetProgressEvent, SetProgressEvent));
            FastSetProperty("onupdateready", Engine.AsProperty(GetUpdateReadyEvent, SetUpdateReadyEvent));
            FastSetProperty("oncached", Engine.AsProperty(GetCachedEvent, SetCachedEvent));
            FastSetProperty("onobsolete", Engine.AsProperty(GetObsoleteEvent, SetObsoleteEvent));
        }

        public static ApplicationCachePrototype CreatePrototypeObject(EngineInstance engine, ApplicationCacheConstructor constructor)
        {
            var obj = new ApplicationCachePrototype(engine)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Update(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ApplicationCacheInstance>(Fail).RefApplicationCache;
            reference.Update();
            return JsValue.Undefined;
        }

        JsValue Abort(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ApplicationCacheInstance>(Fail).RefApplicationCache;
            reference.Abort();
            return JsValue.Undefined;
        }

        JsValue SwapCache(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<ApplicationCacheInstance>(Fail).RefApplicationCache;
            reference.Swap();
            return JsValue.Undefined;
        }

        JsValue GetStatus(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ApplicationCacheInstance>(Fail).RefApplicationCache;
            return _engine.GetDomNode(reference.Status);
        }


        JsValue GetCheckingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onchecking");
        }

        void SetCheckingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetCheckingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Checking -= method;
            }

            instance.Put("onchecking", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Checking += method;
            }
        }

        JsValue GetErrorEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onerror");
        }

        void SetErrorEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetErrorEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Error -= method;
            }

            instance.Put("onerror", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Error += method;
            }
        }

        JsValue GetNoUpdateEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onnoupdate");
        }

        void SetNoUpdateEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetNoUpdateEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.NoUpdate -= method;
            }

            instance.Put("onnoupdate", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.NoUpdate += method;
            }
        }

        JsValue GetDownloadingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("ondownloading");
        }

        void SetDownloadingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetDownloadingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Downloading -= method;
            }

            instance.Put("ondownloading", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Downloading += method;
            }
        }

        JsValue GetProgressEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onprogress");
        }

        void SetProgressEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetProgressEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Progress -= method;
            }

            instance.Put("onprogress", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Progress += method;
            }
        }

        JsValue GetUpdateReadyEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onupdateready");
        }

        void SetUpdateReadyEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetUpdateReadyEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.UpdateReady -= method;
            }

            instance.Put("onupdateready", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.UpdateReady += method;
            }
        }

        JsValue GetCachedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("oncached");
        }

        void SetCachedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetCachedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Cached -= method;
            }

            instance.Put("oncached", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Cached += method;
            }
        }

        JsValue GetObsoleteEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            return instance.Get("onobsolete");
        }

        void SetObsoleteEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<ApplicationCacheInstance>(Fail);
            var reference = instance.RefApplicationCache;
            var existing = GetObsoleteEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Obsolete -= method;
            }

            instance.Put("onobsolete", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Obsolete += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object ApplicationCache]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}