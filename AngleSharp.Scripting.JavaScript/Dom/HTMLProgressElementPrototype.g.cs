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

    sealed partial class HTMLProgressElementPrototype : HTMLProgressElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLProgressElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("max", Engine.AsProperty(GetMax, SetMax));
            FastSetProperty("position", Engine.AsProperty(GetPosition));
            FastSetProperty("labels", Engine.AsProperty(GetLabels));
        }

        public static HTMLProgressElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLProgressElementConstructor constructor)
        {
            var obj = new HTMLProgressElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Value = value;
        }

        JsValue GetMax(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            return _engine.GetDomNode(reference.Maximum);
        }

        void SetMax(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            var value = TypeConverter.ToNumber(argument);
            reference.Maximum = value;
        }

        JsValue GetPosition(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            return _engine.GetDomNode(reference.Position);
        }


        JsValue GetLabels(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLProgressElementInstance>(Fail).RefHTMLProgressElement;
            return _engine.GetDomNode(reference.Labels);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLProgressElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}