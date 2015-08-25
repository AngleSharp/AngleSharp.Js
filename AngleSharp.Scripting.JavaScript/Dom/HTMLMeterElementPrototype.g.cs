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

    sealed partial class HTMLMeterElementPrototype : HTMLMeterElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLMeterElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("min", Engine.AsProperty(GetMin, SetMin));
            FastSetProperty("max", Engine.AsProperty(GetMax, SetMax));
            FastSetProperty("low", Engine.AsProperty(GetLow, SetLow));
            FastSetProperty("high", Engine.AsProperty(GetHigh, SetHigh));
            FastSetProperty("optimum", Engine.AsProperty(GetOptimum, SetOptimum));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
        }

        public static HTMLMeterElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMeterElementConstructor constructor)
        {
            var obj = new HTMLMeterElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Value = value;
        }

        JsValue GetMin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Minimum);
        }

        void SetMin(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Minimum = value;
        }

        JsValue GetMax(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Maximum);
        }

        void SetMax(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Maximum = value;
        }

        JsValue GetLow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Low);
        }

        void SetLow(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Low = value;
        }

        JsValue GetHigh(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.High);
        }

        void SetHigh(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.High = value;
        }

        JsValue GetOptimum(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Optimum);
        }

        void SetOptimum(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Optimum = value;
        }

        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return _engine.GetDomNode(reference.Labels);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLMeterElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}