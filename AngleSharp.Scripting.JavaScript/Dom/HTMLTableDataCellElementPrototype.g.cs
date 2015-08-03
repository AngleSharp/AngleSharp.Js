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

    sealed partial class HTMLTableDataCellElementPrototype : HTMLTableDataCellElementInstance
    {
        public HTMLTableDataCellElementPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
        }

        public static HTMLTableDataCellElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableDataCellElementConstructor constructor)
        {
            var obj = new HTMLTableDataCellElementPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.HTMLTableCellElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableDataCellElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}