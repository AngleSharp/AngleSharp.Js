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

    sealed partial class HTMLParamElementPrototype : HTMLParamElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLParamElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
        }

        public static HTMLParamElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLParamElementConstructor constructor)
        {
            var obj = new HTMLParamElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLParamElementInstance>(Fail).RefHTMLParamElement;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLParamElementInstance>(Fail).RefHTMLParamElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLParamElementInstance>(Fail).RefHTMLParamElement;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLParamElementInstance>(Fail).RefHTMLParamElement;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLParamElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}