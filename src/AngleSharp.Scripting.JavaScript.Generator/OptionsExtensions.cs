namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Dom.Events;
    using AngleSharp.Media.Dom;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class OptionsExtensions
    {
        public static Options UseJintConverters(this Options options)
        {
            options.TypeConverters.Add(typeof(Int32), "TypeConverter.ToInt32");
            options.TypeConverters.Add(typeof(UInt16), "TypeConverter.ToUint16");
            options.TypeConverters.Add(typeof(UInt32), "TypeConverter.ToUint32");
            options.TypeConverters.Add(typeof(Double), "TypeConverter.ToNumber");
            options.TypeConverters.Add(typeof(Boolean), "TypeConverter.ToBoolean");
            options.TypeConverters.Add(typeof(String), "TypeConverter.ToString");
            return options;
        }

        public static Options UseDomConverters(this Options options)
        {
            options.TypeConverters.Add(typeof(DomEventHandler), "DomTypeConverter.ToEventHandler");
            options.TypeConverters.Add(typeof(MutationCallback), "DomTypeConverter.ToMutationCallback");
            options.TypeConverters.Add(typeof(NodeFilter), "DomTypeConverter.ToNodeFilter");
            options.TypeConverters.Add(typeof(IElement), "DomTypeConverter.ToElement");
            options.TypeConverters.Add(typeof(IAttr), "DomTypeConverter.ToAttr");
            options.TypeConverters.Add(typeof(IHtmlElement), "DomTypeConverter.ToHtmlElement");
            options.TypeConverters.Add(typeof(IHtmlOptionElement), "DomTypeConverter.ToOptionElement");
            options.TypeConverters.Add(typeof(IHtmlTableCaptionElement), "DomTypeConverter.ToTableCaptionElement");
            options.TypeConverters.Add(typeof(IHtmlTableSectionElement), "DomTypeConverter.ToTableSectionElement");
            options.TypeConverters.Add(typeof(INode), "DomTypeConverter.ToNode");
            options.TypeConverters.Add(typeof(IEventTarget), "DomTypeConverter.ToEventTarget");
            options.TypeConverters.Add(typeof(Action<IWindow>), "DomTypeConverter.ToTimer");
            options.TypeConverters.Add(typeof(IRenderingContext), "DomTypeConverter.ToRenderingContext");
            options.TypeConverters.Add(typeof(IWindow), "DomTypeConverter.ToWindow");
            options.TypeConverters.Add(typeof(IDocumentType), "DomTypeConverter.ToDoctype");
            options.TypeConverters.Add(typeof(IHtmlMenuElement), "DomTypeConverter.ToMenuElement");
            options.TypeConverters.Add(typeof(IHtmlOptionsGroupElement), "DomTypeConverter.ToOptionsGroupElement");
            options.TypeConverters.Add(typeof(ITouchList), "DomTypeConverter.ToTouchList");
            options.TypeConverters.Add(typeof(IRange), "DomTypeConverter.ToRange");
            options.TypeConverters.Add(typeof(AdjacentPosition), "DomTypeConverter.ToAdjacentPosition");
            options.TypeConverters.Add(typeof(RangeType), "DomTypeConverter.ToRangeType");
            options.TypeConverters.Add(typeof(WheelMode), "DomTypeConverter.ToWheelMode");
            options.TypeConverters.Add(typeof(MouseButton), "DomTypeConverter.ToMouseButton");
            options.TypeConverters.Add(typeof(KeyboardLocation), "DomTypeConverter.ToKeyboardLocation");
            options.TypeConverters.Add(typeof(TextTrackMode), "DomTypeConverter.ToTextTrackMode");
            options.TypeConverters.Add(typeof(FilterSettings), "DomTypeConverter.ToFilterSettings");
            options.TypeConverters.Add(typeof(ITextTrackCue), "DomTypeConverter.ToTextTrackCue");
            options.TypeConverters.Add(typeof(Event), "DomTypeConverter.ToEvent");
            options.TypeConverters.Add(typeof(DomException), "DomTypeConverter.ToDomException");
            options.TypeConverters.Add(typeof(IMessagePort), "DomTypeConverter.ToMessagePort");
            return options;
        }

        public static Options UseSystemConverters(this Options options)
        {
            options.TypeConverters.Add(typeof(IDictionary<String, Object>), "SystemTypeConverter.ToObjBag");
            options.TypeConverters.Add(typeof(Action<Stream>), "SystemTypeConverter.ToStreamTask");
            options.TypeConverters.Add(typeof(Nullable<Int32>), "SystemTypeConverter.ToOptionalInt32");
            options.TypeConverters.Add(typeof(Nullable<DateTime>), "SystemTypeConverter.ToOptionalDateTime");
            options.TypeConverters.Add(typeof(Object), "SystemTypeConverter.ToObject");
            return options;
        }
    }
}
