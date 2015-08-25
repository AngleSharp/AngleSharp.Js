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

    sealed partial class HTMLImageElementPrototype : HTMLImageElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLImageElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("alt", Engine.AsProperty(GetAlt, SetAlt));
            FastSetProperty("currentSrc", Engine.AsProperty(GetCurrentSrc));
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("srcset", Engine.AsProperty(GetSrcset, SetSrcset));
            FastSetProperty("sizes", Engine.AsProperty(GetSizes, SetSizes));
            FastSetProperty("crossOrigin", Engine.AsProperty(GetCrossOrigin, SetCrossOrigin));
            FastSetProperty("useMap", Engine.AsProperty(GetUseMap, SetUseMap));
            FastSetProperty("isMap", Engine.AsProperty(GetIsMap, SetIsMap));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
            FastSetProperty("naturalWidth", Engine.AsProperty(GetNaturalWidth));
            FastSetProperty("naturalHeight", Engine.AsProperty(GetNaturalHeight));
            FastSetProperty("complete", Engine.AsProperty(GetComplete));
        }

        public static HTMLImageElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLImageElementConstructor constructor)
        {
            var obj = new HTMLImageElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetAlt(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.AlternativeText);
        }

        void SetAlt(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.AlternativeText = value;
        }

        JsValue GetCurrentSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.ActualSource);
        }


        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetSrcset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.SourceSet);
        }

        void SetSrcset(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.SourceSet = value;
        }

        JsValue GetSizes(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.Sizes);
        }

        void SetSizes(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.Sizes = value;
        }

        JsValue GetCrossOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.CrossOrigin);
        }

        void SetCrossOrigin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.CrossOrigin = value;
        }

        JsValue GetUseMap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.UseMap);
        }

        void SetUseMap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToString(argument);
            reference.UseMap = value;
        }

        JsValue GetIsMap(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.IsMap);
        }

        void SetIsMap(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsMap = value;
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.DisplayWidth);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayWidth = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.DisplayHeight);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            var value = TypeConverter.ToInt32(argument);
            reference.DisplayHeight = value;
        }

        JsValue GetNaturalWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.OriginalWidth);
        }


        JsValue GetNaturalHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.OriginalHeight);
        }


        JsValue GetComplete(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLImageElementInstance>(Fail).RefHTMLImageElement;
            return _engine.GetDomNode(reference.IsCompleted);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLImageElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}