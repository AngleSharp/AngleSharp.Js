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

    sealed partial class HTMLFieldSetElementPrototype : HTMLFieldSetElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLFieldSetElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
            FastSetProperty("form", Engine.AsProperty(GetForm));
            FastSetProperty("name", Engine.AsProperty(GetName, SetName));
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("elements", Engine.AsProperty(GetElements));
        }

        public static HTMLFieldSetElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLFieldSetElementConstructor constructor)
        {
            var obj = new HTMLFieldSetElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            return _engine.GetDomNode(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue GetForm(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            return _engine.GetDomNode(reference.Form);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            return _engine.GetDomNode(reference.Name);
        }

        void SetName(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            var value = TypeConverter.ToString(argument);
            reference.Name = value;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            return _engine.GetDomNode(reference.Type);
        }


        JsValue GetElements(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLFieldSetElementInstance>(Fail).RefHTMLFieldSetElement;
            return _engine.GetDomNode(reference.Elements);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLFieldSetElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}