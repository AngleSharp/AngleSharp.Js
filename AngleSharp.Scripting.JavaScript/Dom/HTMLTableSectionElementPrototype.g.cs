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

    sealed partial class HTMLTableSectionElementPrototype : HTMLTableSectionElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableSectionElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("insertRow", Engine.AsValue(InsertRow), true, true, true);
            FastAddProperty("deleteRow", Engine.AsValue(DeleteRow), true, true, true);
            FastSetProperty("rows", Engine.AsProperty(GetRows));
        }

        public static HTMLTableSectionElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableSectionElementConstructor constructor)
        {
            var obj = new HTMLTableSectionElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InsertRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableSectionElementInstance>(Fail).RefHTMLTableSectionElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return _engine.GetDomNode(reference.InsertRowAt(index));
        }

        JsValue DeleteRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableSectionElementInstance>(Fail).RefHTMLTableSectionElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveRowAt(index);
            return JsValue.Undefined;
        }

        JsValue GetRows(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableSectionElementInstance>(Fail).RefHTMLTableSectionElement;
            return _engine.GetDomNode(reference.Rows);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableSectionElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}