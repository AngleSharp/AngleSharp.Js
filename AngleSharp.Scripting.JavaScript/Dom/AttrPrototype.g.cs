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

    sealed partial class AttrPrototype : AttrInstance
    {
        readonly EngineInstance _engine;

        public AttrPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("localName", Engine.AsProperty(GetLocalName));
            FastSetProperty("name", Engine.AsProperty(GetName));
            FastSetProperty("value", Engine.AsProperty(GetValue, SetValue));
            FastSetProperty("namespaceURI", Engine.AsProperty(GetNamespaceURI));
            FastSetProperty("prefix", Engine.AsProperty(GetPrefix));
        }

        public static AttrPrototype CreatePrototypeObject(EngineInstance engine, AttrConstructor constructor)
        {
            var obj = new AttrPrototype(engine)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetLocalName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            return _engine.GetDomNode(reference.LocalName);
        }


        JsValue GetName(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            return _engine.GetDomNode(reference.Name);
        }


        JsValue GetValue(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            return _engine.GetDomNode(reference.Value);
        }

        void SetValue(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            var value = TypeConverter.ToString(argument);
            reference.Value = value;
        }

        JsValue GetNamespaceURI(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            return _engine.GetDomNode(reference.NamespaceUri);
        }


        JsValue GetPrefix(JsValue thisObj)
        {
            var reference = thisObj.TryCast<AttrInstance>(Fail).RefAttr;
            return _engine.GetDomNode(reference.Prefix);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Attr]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}