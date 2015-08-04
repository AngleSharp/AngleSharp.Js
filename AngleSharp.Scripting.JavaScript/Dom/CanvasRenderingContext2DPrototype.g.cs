namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Media;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class CanvasRenderingContext2DPrototype : CanvasRenderingContext2DInstance
    {
        public CanvasRenderingContext2DPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("save", Engine.AsValue(Save), true, true, true);
            FastAddProperty("restore", Engine.AsValue(Restore), true, true, true);
            FastSetProperty("canvas", Engine.AsProperty(GetCanvas));
            FastSetProperty("width", Engine.AsProperty(GetWidth, SetWidth));
            FastSetProperty("height", Engine.AsProperty(GetHeight, SetHeight));
        }

        public static CanvasRenderingContext2DPrototype CreatePrototypeObject(EngineInstance engine, CanvasRenderingContext2DConstructor constructor)
        {
            var obj = new CanvasRenderingContext2DPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.RenderingContext.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Save(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            reference.SaveState();
            return JsValue.Undefined;
        }

        JsValue Restore(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            reference.RestoreState();
            return JsValue.Undefined;
        }

        JsValue GetCanvas(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            return Engine.Select(reference.Canvas);
        }


        JsValue GetWidth(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            return Engine.Select(reference.Width);
        }

        void SetWidth(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            var value = TypeConverter.ToInt32(argument);
            reference.Width = value;
        }

        JsValue GetHeight(JsValue thisObj)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            return Engine.Select(reference.Height);
        }

        void SetHeight(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<CanvasRenderingContext2DInstance>(Fail).RefCanvasRenderingContext2D;
            var value = TypeConverter.ToInt32(argument);
            reference.Height = value;
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object CanvasRenderingContext2D]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}