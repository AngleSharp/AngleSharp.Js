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

    sealed partial class HTMLBodyElementPrototype : HTMLBodyElementInstance
    {
        public HTMLBodyElementPrototype(Engine engine)
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
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
            FastSetProperty("onafterprint", Engine.AsProperty(GetPrintedEvent, SetPrintedEvent));
            FastSetProperty("onbeforeprint", Engine.AsProperty(GetPrintingEvent, SetPrintingEvent));
            FastSetProperty("onbeforeunload", Engine.AsProperty(GetUnloadingEvent, SetUnloadingEvent));
            FastSetProperty("onhashchange", Engine.AsProperty(GetHashChangedEvent, SetHashChangedEvent));
            FastSetProperty("onmessage", Engine.AsProperty(GetMessageReceivedEvent, SetMessageReceivedEvent));
            FastSetProperty("onoffline", Engine.AsProperty(GetWentOfflineEvent, SetWentOfflineEvent));
            FastSetProperty("ononline", Engine.AsProperty(GetWentOnlineEvent, SetWentOnlineEvent));
            FastSetProperty("onpagehide", Engine.AsProperty(GetPageHiddenEvent, SetPageHiddenEvent));
            FastSetProperty("onpageshow", Engine.AsProperty(GetPageShownEvent, SetPageShownEvent));
            FastSetProperty("onpopstate", Engine.AsProperty(GetPopStateEvent, SetPopStateEvent));
            FastSetProperty("onstorage", Engine.AsProperty(GetStorageEvent, SetStorageEvent));
            FastSetProperty("onunload", Engine.AsProperty(GetUnloadedEvent, SetUnloadedEvent));
        }

        public static HTMLBodyElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLBodyElementConstructor constructor)
        {
            var obj = new HTMLBodyElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLBodyElementInstance>(Fail).RefHTMLBodyElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
        }

        JsValue GetPrintedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onafterprint");
        }

        void SetPrintedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetPrintedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Printed -= method;
            }

            instance.Put("onafterprint", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Printed += method;
            }
        }

        JsValue GetPrintingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onbeforeprint");
        }

        void SetPrintingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetPrintingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Printing -= method;
            }

            instance.Put("onbeforeprint", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Printing += method;
            }
        }

        JsValue GetUnloadingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onbeforeunload");
        }

        void SetUnloadingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetUnloadingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Unloading -= method;
            }

            instance.Put("onbeforeunload", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Unloading += method;
            }
        }

        JsValue GetHashChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onhashchange");
        }

        void SetHashChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetHashChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.HashChanged -= method;
            }

            instance.Put("onhashchange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.HashChanged += method;
            }
        }

        JsValue GetMessageReceivedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onmessage");
        }

        void SetMessageReceivedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetMessageReceivedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MessageReceived -= method;
            }

            instance.Put("onmessage", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MessageReceived += method;
            }
        }

        JsValue GetWentOfflineEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onoffline");
        }

        void SetWentOfflineEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetWentOfflineEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.WentOffline -= method;
            }

            instance.Put("onoffline", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.WentOffline += method;
            }
        }

        JsValue GetWentOnlineEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("ononline");
        }

        void SetWentOnlineEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetWentOnlineEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.WentOnline -= method;
            }

            instance.Put("ononline", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.WentOnline += method;
            }
        }

        JsValue GetPageHiddenEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onpagehide");
        }

        void SetPageHiddenEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetPageHiddenEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.PageHidden -= method;
            }

            instance.Put("onpagehide", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.PageHidden += method;
            }
        }

        JsValue GetPageShownEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onpageshow");
        }

        void SetPageShownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetPageShownEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.PageShown -= method;
            }

            instance.Put("onpageshow", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.PageShown += method;
            }
        }

        JsValue GetPopStateEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onpopstate");
        }

        void SetPopStateEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetPopStateEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.PopState -= method;
            }

            instance.Put("onpopstate", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.PopState += method;
            }
        }

        JsValue GetStorageEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onstorage");
        }

        void SetStorageEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetStorageEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Storage -= method;
            }

            instance.Put("onstorage", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Storage += method;
            }
        }

        JsValue GetUnloadedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            return instance.Get("onunload");
        }

        void SetUnloadedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<HTMLBodyElementInstance>(Fail);
            var reference = instance.RefHTMLBodyElement;
            var existing = GetUnloadedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Unloaded -= method;
            }

            instance.Put("onunload", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Unloaded += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLBodyElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}