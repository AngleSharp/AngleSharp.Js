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

    sealed partial class StyleSheetPrototype : StyleSheetInstance
    {
        public StyleSheetPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("type", Engine.AsProperty(GetType));
            FastSetProperty("href", Engine.AsProperty(GetHref));
            FastSetProperty("ownerNode", Engine.AsProperty(GetOwnerNode));
            FastSetProperty("parentStyleSheet", Engine.AsProperty(GetParentStyleSheet));
            FastSetProperty("title", Engine.AsProperty(GetTitle));
            FastSetProperty("media", Engine.AsProperty(GetMedia, SetMedia));
            FastSetProperty("disabled", Engine.AsProperty(GetDisabled, SetDisabled));
        }

        public static StyleSheetPrototype CreatePrototypeObject(EngineInstance engine, StyleSheetConstructor constructor)
        {
            var obj = new StyleSheetPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.Type);
        }


        JsValue GetHref(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.Href);
        }


        JsValue GetOwnerNode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.OwnerNode);
        }


        JsValue GetParentStyleSheet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.Parent);
        }


        JsValue GetTitle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.Title);
        }


        JsValue GetMedia(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.Media);
        }

        void SetMedia(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            var value = TypeConverter.ToString(argument);
            reference.Media.MediaText = value;
        }

        JsValue GetDisabled(JsValue thisObj)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            return Engine.Select(reference.IsDisabled);
        }

        void SetDisabled(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<StyleSheetInstance>(Fail).RefStyleSheet;
            var value = TypeConverter.ToBoolean(argument);
            reference.IsDisabled = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object StyleSheet]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}