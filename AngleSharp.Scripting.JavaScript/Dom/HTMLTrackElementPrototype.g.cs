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

    sealed partial class HTMLTrackElementPrototype : HTMLTrackElementInstance
    {
        public HTMLTrackElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("kind", Engine.AsProperty(GetKind, SetKind));
            FastSetProperty("src", Engine.AsProperty(GetSrc, SetSrc));
            FastSetProperty("srclang", Engine.AsProperty(GetSrclang, SetSrclang));
            FastSetProperty("label", Engine.AsProperty(GetLabel, SetLabel));
            FastSetProperty("default", Engine.AsProperty(GetDefault, SetDefault));
            FastSetProperty("readyState", Engine.AsProperty(GetReadyState));
            FastSetProperty("track", Engine.AsProperty(GetTrack));
        }

        public static HTMLTrackElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTrackElementConstructor constructor)
        {
            var obj = new HTMLTrackElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetKind(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.Kind);
        }

        void SetKind(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            var value = TypeConverter.ToString(argument);
            reference.Kind = value;
        }

        JsValue GetSrc(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.Source);
        }

        void SetSrc(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            var value = TypeConverter.ToString(argument);
            reference.Source = value;
        }

        JsValue GetSrclang(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.SourceLanguage);
        }

        void SetSrclang(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            var value = TypeConverter.ToString(argument);
            reference.SourceLanguage = value;
        }

        JsValue GetLabel(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.Label);
        }

        void SetLabel(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            var value = TypeConverter.ToString(argument);
            reference.Label = value;
        }

        JsValue GetDefault(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.IsDefault);
        }

        void SetDefault(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDefault = value;
        }

        JsValue GetReadyState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.ReadyState);
        }


        JsValue GetTrack(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTrackElementInstance>(Fail).RefHTMLTrackElement;
            return Engine.Select(reference.Track);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTrackElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}