namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class TouchPrototype : TouchInstance
    {
        readonly EngineInstance _engine;

        public TouchPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("identifier", Engine.AsProperty(GetIdentifier));
            FastSetProperty("target", Engine.AsProperty(GetTarget));
            FastSetProperty("screenX", Engine.AsProperty(GetScreenX));
            FastSetProperty("screenY", Engine.AsProperty(GetScreenY));
            FastSetProperty("clientX", Engine.AsProperty(GetClientX));
            FastSetProperty("clientY", Engine.AsProperty(GetClientY));
            FastSetProperty("pageX", Engine.AsProperty(GetPageX));
            FastSetProperty("pageY", Engine.AsProperty(GetPageY));
        }

        public static TouchPrototype CreatePrototypeObject(EngineInstance engine, TouchConstructor constructor)
        {
            var obj = new TouchPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetIdentifier(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.Id);
        }


        JsValue GetTarget(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.Target);
        }


        JsValue GetScreenX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.ScreenX);
        }


        JsValue GetScreenY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.ScreenY);
        }


        JsValue GetClientX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.ClientX);
        }


        JsValue GetClientY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.ClientY);
        }


        JsValue GetPageX(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.PageX);
        }


        JsValue GetPageY(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TouchInstance>(Fail).RefTouch;
            return _engine.GetDomNode(reference.PageY);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Touch]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}