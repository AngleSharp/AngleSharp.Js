namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class RangePrototype : RangeInstance
    {
        readonly EngineInstance _engine;

        public RangePrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("setStart", Engine.AsValue(SetStart), true, true, true);
            FastAddProperty("setEnd", Engine.AsValue(SetEnd), true, true, true);
            FastAddProperty("setStartBefore", Engine.AsValue(SetStartBefore), true, true, true);
            FastAddProperty("setEndBefore", Engine.AsValue(SetEndBefore), true, true, true);
            FastAddProperty("setStartAfter", Engine.AsValue(SetStartAfter), true, true, true);
            FastAddProperty("setEndAfter", Engine.AsValue(SetEndAfter), true, true, true);
            FastAddProperty("collapse", Engine.AsValue(Collapse), true, true, true);
            FastAddProperty("selectNode", Engine.AsValue(SelectNode), true, true, true);
            FastAddProperty("selectNodeContents", Engine.AsValue(SelectNodeContents), true, true, true);
            FastAddProperty("deleteContents", Engine.AsValue(DeleteContents), true, true, true);
            FastAddProperty("extractContents", Engine.AsValue(ExtractContents), true, true, true);
            FastAddProperty("cloneContents", Engine.AsValue(CloneContents), true, true, true);
            FastAddProperty("insertNode", Engine.AsValue(InsertNode), true, true, true);
            FastAddProperty("surroundContents", Engine.AsValue(SurroundContents), true, true, true);
            FastAddProperty("cloneRange", Engine.AsValue(CloneRange), true, true, true);
            FastAddProperty("detach", Engine.AsValue(Detach), true, true, true);
            FastAddProperty("isPointInRange", Engine.AsValue(IsPointInRange), true, true, true);
            FastAddProperty("compareBoundaryPoints", Engine.AsValue(CompareBoundaryPoints), true, true, true);
            FastAddProperty("comparePoint", Engine.AsValue(ComparePoint), true, true, true);
            FastAddProperty("intersectsNode", Engine.AsValue(IntersectsNode), true, true, true);
            FastSetProperty("startContainer", Engine.AsProperty(GetStartContainer));
            FastSetProperty("startOffset", Engine.AsProperty(GetStartOffset));
            FastSetProperty("endContainer", Engine.AsProperty(GetEndContainer));
            FastSetProperty("endOffset", Engine.AsProperty(GetEndOffset));
            FastSetProperty("collapsed", Engine.AsProperty(GetCollapsed));
            FastSetProperty("commonAncestorContainer", Engine.AsProperty(GetCommonAncestorContainer));
        }

        public static RangePrototype CreatePrototypeObject(EngineInstance engine, RangeConstructor constructor)
        {
            var obj = new RangePrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue SetStart(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            var offset = TypeConverter.ToInt32(arguments.At(1));
            reference.StartWith(refNode, offset);
            return JsValue.Undefined;
        }

        JsValue SetEnd(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            var offset = TypeConverter.ToInt32(arguments.At(1));
            reference.EndWith(refNode, offset);
            return JsValue.Undefined;
        }

        JsValue SetStartBefore(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.StartBefore(refNode);
            return JsValue.Undefined;
        }

        JsValue SetEndBefore(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.EndBefore(refNode);
            return JsValue.Undefined;
        }

        JsValue SetStartAfter(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.StartAfter(refNode);
            return JsValue.Undefined;
        }

        JsValue SetEndAfter(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.EndAfter(refNode);
            return JsValue.Undefined;
        }

        JsValue Collapse(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var toStart = TypeConverter.ToBoolean(arguments.At(0));
            reference.Collapse(toStart);
            return JsValue.Undefined;
        }

        JsValue SelectNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.Select(refNode);
            return JsValue.Undefined;
        }

        JsValue SelectNodeContents(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var refNode = DomTypeConverter.ToNode(arguments.At(0));
            reference.SelectContent(refNode);
            return JsValue.Undefined;
        }

        JsValue DeleteContents(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            reference.ClearContent();
            return JsValue.Undefined;
        }

        JsValue ExtractContents(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.ExtractContent());
        }

        JsValue CloneContents(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.CopyContent());
        }

        JsValue InsertNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var node = DomTypeConverter.ToNode(arguments.At(0));
            reference.Insert(node);
            return JsValue.Undefined;
        }

        JsValue SurroundContents(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var newParent = DomTypeConverter.ToNode(arguments.At(0));
            reference.Surround(newParent);
            return JsValue.Undefined;
        }

        JsValue CloneRange(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.Clone());
        }

        JsValue Detach(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            reference.Detach();
            return JsValue.Undefined;
        }

        JsValue IsPointInRange(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var node = DomTypeConverter.ToNode(arguments.At(0));
            var offset = TypeConverter.ToInt32(arguments.At(1));
            return _engine.GetDomNode(reference.Contains(node, offset));
        }

        JsValue CompareBoundaryPoints(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var how = DomTypeConverter.ToRangeType(arguments.At(0));
            var sourceRange = DomTypeConverter.ToRange(arguments.At(1));
            return _engine.GetDomNode(reference.CompareBoundaryTo(how, sourceRange));
        }

        JsValue ComparePoint(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var node = DomTypeConverter.ToNode(arguments.At(0));
            var offset = TypeConverter.ToInt32(arguments.At(1));
            return _engine.GetDomNode(reference.CompareTo(node, offset));
        }

        JsValue IntersectsNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            var node = DomTypeConverter.ToNode(arguments.At(0));
            return _engine.GetDomNode(reference.Intersects(node));
        }

        JsValue GetStartContainer(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.Head);
        }


        JsValue GetStartOffset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.Start);
        }


        JsValue GetEndContainer(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.Tail);
        }


        JsValue GetEndOffset(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.End);
        }


        JsValue GetCollapsed(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.IsCollapsed);
        }


        JsValue GetCommonAncestorContainer(JsValue thisObj)
        {
            var reference = thisObj.TryCast<RangeInstance>(Fail).RefRange;
            return _engine.GetDomNode(reference.CommonAncestor);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Range]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}