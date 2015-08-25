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

    sealed partial class HTMLTableElementPrototype : HTMLTableElementInstance
    {
        readonly EngineInstance _engine;

        public HTMLTableElementPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("createCaption", Engine.AsValue(CreateCaption), true, true, true);
            FastAddProperty("deleteCaption", Engine.AsValue(DeleteCaption), true, true, true);
            FastAddProperty("createTHead", Engine.AsValue(CreateTHead), true, true, true);
            FastAddProperty("deleteTHead", Engine.AsValue(DeleteTHead), true, true, true);
            FastAddProperty("createTFoot", Engine.AsValue(CreateTFoot), true, true, true);
            FastAddProperty("deleteTFoot", Engine.AsValue(DeleteTFoot), true, true, true);
            FastAddProperty("createTBody", Engine.AsValue(CreateTBody), true, true, true);
            FastAddProperty("insertRow", Engine.AsValue(InsertRow), true, true, true);
            FastAddProperty("deleteRow", Engine.AsValue(DeleteRow), true, true, true);
            FastSetProperty("caption", Engine.AsProperty(GetCaption, SetCaption));
            FastSetProperty("tHead", Engine.AsProperty(GetTHead, SetTHead));
            FastSetProperty("tFoot", Engine.AsProperty(GetTFoot, SetTFoot));
            FastSetProperty("tBodies", Engine.AsProperty(GetTBodies));
            FastSetProperty("rows", Engine.AsProperty(GetRows));
            FastSetProperty("border", Engine.AsProperty(GetBorder, SetBorder));
        }

        public static HTMLTableElementPrototype CreatePrototypeObject(EngineInstance engine, HTMLTableElementConstructor constructor)
        {
            var obj = new HTMLTableElementPrototype(engine)
            {
                Prototype = engine.Constructors.HTMLElement.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue CreateCaption(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.CreateCaption());
        }

        JsValue DeleteCaption(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteCaption();
            return JsValue.Undefined;
        }

        JsValue CreateTHead(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.CreateHead());
        }

        JsValue DeleteTHead(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteHead();
            return JsValue.Undefined;
        }

        JsValue CreateTFoot(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.CreateFoot());
        }

        JsValue DeleteTFoot(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            reference.DeleteFoot();
            return JsValue.Undefined;
        }

        JsValue CreateTBody(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.CreateBody());
        }

        JsValue InsertRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            return _engine.GetDomNode(reference.InsertRowAt(index));
        }

        JsValue DeleteRow(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var index = TypeConverter.ToInt32(arguments.At(0));
            reference.RemoveRowAt(index);
            return JsValue.Undefined;
        }

        JsValue GetCaption(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Caption);
        }

        void SetCaption(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableCaptionElement(argument);
            reference.Caption = value;
        }

        JsValue GetTHead(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Head);
        }

        void SetTHead(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableSectionElement(argument);
            reference.Head = value;
        }

        JsValue GetTFoot(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Foot);
        }

        void SetTFoot(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = DomTypeConverter.ToTableSectionElement(argument);
            reference.Foot = value;
        }

        JsValue GetTBodies(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Bodies);
        }


        JsValue GetRows(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Rows);
        }


        JsValue GetBorder(JsValue thisObj)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            return _engine.GetDomNode(reference.Border);
        }

        void SetBorder(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<HTMLTableElementInstance>(Fail).RefHTMLTableElement;
            var value = TypeConverter.ToUint32(argument);
            reference.Border = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object HTMLTableElement]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}