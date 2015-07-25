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
    }
}
