namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Dom;
    using System;

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
            options.TypeConverters.Add(typeof(IElement), "DomTypeConverter.ToElement");
            return options;
        }

        public static Options UseSystemConverters(this Options options)
        {
            options.TypeConverters.Add(typeof(Action<>), "SystemTypeConverter.ToAction");
            return options;
        }
    }
}
