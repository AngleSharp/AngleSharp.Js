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

    sealed partial class TreeWalkerPrototype : TreeWalkerInstance
    {
        readonly EngineInstance _engine;

        public TreeWalkerPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("nextNode", Engine.AsValue(NextNode), true, true, true);
            FastAddProperty("previousNode", Engine.AsValue(PreviousNode), true, true, true);
            FastAddProperty("parentNode", Engine.AsValue(ParentNode), true, true, true);
            FastAddProperty("firstChild", Engine.AsValue(FirstChild), true, true, true);
            FastAddProperty("lastChild", Engine.AsValue(LastChild), true, true, true);
            FastAddProperty("previousSibling", Engine.AsValue(PreviousSibling), true, true, true);
            FastAddProperty("nextSibling", Engine.AsValue(NextSibling), true, true, true);
            FastSetProperty("root", Engine.AsProperty(GetRoot));
            FastSetProperty("currentNode", Engine.AsProperty(GetCurrentNode, SetCurrentNode));
            FastSetProperty("whatToShow", Engine.AsProperty(GetWhatToShow));
            FastSetProperty("filter", Engine.AsProperty(GetFilter));
        }

        public static TreeWalkerPrototype CreatePrototypeObject(EngineInstance engine, TreeWalkerConstructor constructor)
        {
            var obj = new TreeWalkerPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue NextNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToNext());
        }

        JsValue PreviousNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToPrevious());
        }

        JsValue ParentNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToParent());
        }

        JsValue FirstChild(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToFirst());
        }

        JsValue LastChild(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToLast());
        }

        JsValue PreviousSibling(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToPreviousSibling());
        }

        JsValue NextSibling(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.ToNextSibling());
        }

        JsValue GetRoot(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.Root);
        }


        JsValue GetCurrentNode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.Current);
        }

        void SetCurrentNode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            var value = DomTypeConverter.ToNode(argument);
            reference.Current = value;
        }

        JsValue GetWhatToShow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.Settings);
        }


        JsValue GetFilter(JsValue thisObj)
        {
            var reference = thisObj.TryCast<TreeWalkerInstance>(Fail).RefTreeWalker;
            return _engine.GetDomNode(reference.Filter);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object TreeWalker]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}