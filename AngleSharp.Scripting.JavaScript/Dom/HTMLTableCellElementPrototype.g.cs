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

    sealed partial class HTMLTableCellElementPrototype : HTMLTableCellElementInstance
    {
        public HTMLTableCellElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("colSpan", Engine.AsProperty(GetColSpan, SetColSpan));
            FastSetProperty("rowSpan", Engine.AsProperty(GetRowSpan, SetRowSpan));
            FastSetProperty("headers", Engine.AsProperty(GetHeaders));
            FastSetProperty("cellIndex", Engine.AsProperty(GetCellIndex));
        }

        public static HTMLTableCellElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableCellElementConstructor constructor)
        {
            var obj = new HTMLTableCellElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetColSpan(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            return Engine.Select(reference.ColumnSpan);
        }

        void SetColSpan(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            var value = TypeConverter.ToInt32(argument);
            reference.ColumnSpan = value;
        }

        JsValue GetRowSpan(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            return Engine.Select(reference.RowSpan);
        }

        void SetRowSpan(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            var value = TypeConverter.ToInt32(argument);
            reference.RowSpan = value;
        }

        JsValue GetHeaders(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            return Engine.Select(reference.Headers);
        }


        JsValue GetCellIndex(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableCellElementInstance>(Fail).RefHTMLTableCellElement;
            return Engine.Select(reference.Index);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableCellElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}