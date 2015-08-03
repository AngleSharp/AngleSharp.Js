namespace AngleSharp.Scripting.JavaScript
{
    using Jint.Native;
    using System;
    using System.Collections.Generic;
    using System.IO;

    static class SystemTypeConverter
    {
        public static IDictionary<String, Object> ToObjBag(JsValue arg)
        {
            return null;
        }

        public static Action<Stream> ToStreamTask(JsValue arg)
        {
            return null;
        }

        public static Nullable<Int32> ToOptionalInt32(JsValue arg)
        {
            return null;
        }

        public static Nullable<DateTime> ToOptionalDateTime(JsValue arg)
        {
            return null;
        }

        public static Object ToObject(JsValue arg)
        {
            return null;
        }

        public static String[] ToStringArray(JsValue arg)
        {
            return null;
        }
    }
}
