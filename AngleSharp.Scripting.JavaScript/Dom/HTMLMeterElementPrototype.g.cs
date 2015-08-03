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
        public HTMLMeterElementPrototype(Engine engine)
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
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("min", Engine.AsProperty(GetMin, SetMin));
            FastSetProperty("max", Engine.AsProperty(GetMax, SetMax));
            FastSetProperty("low", Engine.AsProperty(GetLow, SetLow));
            FastSetProperty("high", Engine.AsProperty(GetHigh, SetHigh));
            FastSetProperty("optimum", Engine.AsProperty(GetOptimum, SetOptimum));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("nextElementSibling", Engine.AsProperty(GetNextElementSibling));
            FastSetProperty("previousElementSibling", Engine.AsProperty(GetPreviousElementSibling));
            FastSetProperty("style", Engine.AsProperty(GetStyle, SetStyle));
        }

        public static HTMLMeterElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLMeterElementConstructor constructor)
        {
            var obj = new HTMLMeterElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue Before(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Before(nodes);
            return JsValue.Undefined;
        }

        JsValue After(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.After(nodes);
            return JsValue.Undefined;
        }

        JsValue Replace(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Replace(nodes);
            return JsValue.Undefined;
        }

        JsValue Remove(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            reference.Remove();
            return JsValue.Undefined;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.Value);
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
            return Engine.Select(reference.Minimum);
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
            return Engine.Select(reference.Maximum);
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
            return Engine.Select(reference.Low);
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
            return Engine.Select(reference.High);
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
            return Engine.Select(reference.Optimum);
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
            return Engine.Select(reference.Labels);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetNextElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.NextElementSibling);
        }


        JsValue GetPreviousElementSibling(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.PreviousElementSibling);
        }


        JsValue GetStyle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            return Engine.Select(reference.Style);
        }

        void SetStyle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLMeterElementInstance>(Fail).RefHTMLMeterElement;
            var value = TypeConverter.ToString(argument);
            reference.Style.CssText = value;
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