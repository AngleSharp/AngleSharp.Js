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

    sealed partial class HTMLCanvasElementPrototype : HTMLCanvasElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLCanvasElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("toDataURL", Engine.AsValue(ToDataURL), true, true, true);
            FastAddProperty("toBlob", Engine.AsValue(ToBlob), true, true, true);
            FastAddProperty("getContext", Engine.AsValue(GetContext), true, true, true);
            FastAddProperty("setContext", Engine.AsValue(SetContext), true, true, true);
            FastAddProperty("probablySupportsContext", Engine.AsValue(ProbablySupportsContext), true, true, true);
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
        }

        public static HTMLCanvasElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLCanvasElementConstructor constructor)
        {
            var obj = new HTMLCanvasElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToDataURL(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var type = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.ToDataUrl(type));
        }

        JsValue ToBlob(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var callback = SystemTypeConverter.ToStreamTask(arguments.At(0));
            var type = TypeConverter.ToString(arguments.At(1));
            reference.ToBlob(callback, type);
            return JsValue.Undefined;
        }

        JsValue GetContext(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var contextId = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.GetContext(contextId));
        }

        JsValue SetContext(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var context = DomTypeConverter.ToRenderingContext(arguments.At(0));
            reference.SetContext(context);
            return JsValue.Undefined;
        }

        JsValue ProbablySupportsContext(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var contextId = TypeConverter.ToString(arguments.At(0));
            return _engine.GetDomNode(reference.IsSupportingContext(contextId));
        }

        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            return _engine.GetDomNode(reference.Width);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Width = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            return _engine.GetDomNode(reference.Height);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLCanvasElementInstance>(Fail).RefHTMLCanvasElement;
            var value = TypeConverter.ToInt32(argument);
            reference.Height = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLCanvasElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}