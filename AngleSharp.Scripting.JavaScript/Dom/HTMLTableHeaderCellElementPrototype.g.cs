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

    sealed partial class HTMLTableHeaderCellElementPrototype : HTMLTableHeaderCellElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableHeaderCellElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("scope", Engine.AsProperty(GetScope, SetScope));
        }

        public static HTMLTableHeaderCellElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableHeaderCellElementConstructor constructor)
        {
            var obj = new HTMLTableHeaderCellElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLTableCellElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetScope(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableHeaderCellElementInstance>(Fail).RefHTMLTableHeaderCellElement;
            return _engine.GetDomNode(reference.Scope);
        }

        void SetScope(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableHeaderCellElementInstance>(Fail).RefHTMLTableHeaderCellElement;
            var value = TypeConverter.ToString(argument);
            reference.Scope = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableHeaderCellElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}