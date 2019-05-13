namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Dom.Events;
    using AngleSharp.Media.Dom;
    using Jint.Native;
    using System;

    static class DomTypeConverter
    {
        public static DomEventHandler ToEventHandler(JsValue arg)
        {
            return null;
        }

        public static MutationCallback ToMutationCallback(JsValue arg)
        {
            return null;
        }

        public static NodeFilter ToNodeFilter(JsValue arg)
        {
            return null;
        }

        public static IAttr ToAttr(JsValue arg)
        {
            return null;
        }

        public static IElement ToElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlElement ToHtmlElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlOptionElement ToOptionElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlTableCaptionElement ToTableCaptionElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlTableSectionElement ToTableSectionElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlMenuElement ToMenuElement(JsValue arg)
        {
            return null;
        }

        public static IHtmlOptionsGroupElement ToOptionsGroupElement(JsValue arg)
        {
            return null;
        }

        public static IEventTarget ToEventTarget(JsValue arg)
        {
            return null;
        }

        public static INode ToNode(JsValue arg)
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

        public static IWindow ToWindow(JsValue arg)
        {
            return null;
        }

        public static IDocumentType ToDoctype(JsValue arg)
        {
            return null;
        }

        public static IMessagePort ToMessagePort(JsValue arg)
        {
            return null;
        }

        public static DomException ToDomException(JsValue arg)
        {
            return null;
        }

        public static ITouchList ToTouchList(JsValue arg)
        {
            return null;
        }

        public static ITextTrackCue ToTextTrackCue(JsValue arg)
        {
            return null;
        }

        public static Event ToEvent(JsValue arg)
        {
            return null;
        }

        public static IRange ToRange(JsValue arg)
        {
            return null;
        }

        public static AdjacentPosition ToAdjacentPosition(JsValue arg)
        {
            return ToEnum<AdjacentPosition>(arg);
        }

        public static RangeType ToRangeType(JsValue arg)
        {
            return ToEnum<RangeType>(arg);
        }

        public static WheelMode ToWheelMode(JsValue arg)
        {
            return ToEnum<WheelMode>(arg);
        }

        public static MouseButton ToMouseButton(JsValue arg)
        {
            return ToEnum<MouseButton>(arg);
        }

        public static KeyboardLocation ToKeyboardLocation(JsValue arg)
        {
            return ToEnum<KeyboardLocation>(arg);
        }

        public static TextTrackMode ToTextTrackMode(JsValue arg)
        {
            return ToEnum<TextTrackMode>(arg);
        }

        public static FilterSettings ToFilterSettings(JsValue arg)
        {
            return ToEnum<FilterSettings>(arg);
        }

        public static T ToEnum<T>(JsValue arg)
            where T : struct, IComparable
        {
 	        return default(T);
        }
    }
}
