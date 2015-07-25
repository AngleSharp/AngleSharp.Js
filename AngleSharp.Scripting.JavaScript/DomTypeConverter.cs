namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Media;
    using Jint.Native;
    using System;

    static class DomTypeConverter
    {
        public static DomEventHandler ToEventHandler(JsValue arg)
        {
            return null;
        }

        public static IElement ToElement(JsValue arg)
        {
            return null;
        }

        public static INode ToNode(JsValue arg)
        {
            return null;
        }

        public static INode[] ToNodeArray(JsValue arg)
        {
            return null;
        }

        public static Action<IWindow> ToTimer(JsValue arg)
        {
            return null;
        }

        public static IRenderingContext ToRenderingContext(JsValue arg)
        {
            return null;
        }
    }
}
