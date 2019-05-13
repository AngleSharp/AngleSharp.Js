namespace AngleSharp.Js
{
    using Jint.Native;

    static class UnresolvedConverter
    {
        public static T To<T>(JsValue arg)
        {
            return default(T);
        }
    }
}
