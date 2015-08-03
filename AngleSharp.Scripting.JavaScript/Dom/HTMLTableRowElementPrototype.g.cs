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

    sealed partial class HTMLTableRowElementPrototype : HTMLTableRowElementInstance
    {
        public HTMLTableRowElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("insertCell", Engine.AsValue(InsertCell), true, true, true);
            FastAddProperty("deleteCell", Engine.AsValue(DeleteCell), true, true, true);
            FastSetProperty("rowIndex", Engine.AsProperty(GetRowIndex));
            FastSetProperty("sectionRowIndex", Engine.AsProperty(GetSectionRowIndex));
            FastSetProperty("cells", Engine.AsProperty(GetCells));
        }

        public static HTMLTableRowElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableRowElementConstructor constructor)
        {
            var obj = new HTMLTableRowElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue InsertCell(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableRowElementInstance>(Fail).RefHTMLTableRowElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return Engine.Select(reference.InsertCellAt(index));
        }

        JsValue DeleteCell(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableRowElementInstance>(Fail).RefHTMLTableRowElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveCellAt(index);
            return JsValue.Undefined;
        }

        JsValue GetRowIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableRowElementInstance>(Fail).RefHTMLTableRowElement;
            return Engine.Select(reference.Index);
        }


        JsValue GetSectionRowIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableRowElementInstance>(Fail).RefHTMLTableRowElement;
            return Engine.Select(reference.IndexInSection);
        }


        JsValue GetCells(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableRowElementInstance>(Fail).RefHTMLTableRowElement;
            return Engine.Select(reference.Cells);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableRowElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}