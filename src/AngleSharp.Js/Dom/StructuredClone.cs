namespace AngleSharp.Js.Dom
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Array;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A copy of a value for another realm (HTML 2.7), which holds nothing of the one it comes
    /// from: a script object of one engine can't be used by another one, and handing it over would
    /// hand over that engine, too. Functions and DOM objects can't be cloned.
    /// </summary>
    static class StructuredClone
    {
        private const Int32 MaxDepth = 256;

        public static Object Clone(Object value) => Clone(value, new HashSet<Object>(), 0);

        private static Object Clone(Object value, HashSet<Object> path, Int32 depth)
        {
            switch (value)
            {
                case null:
                case String:
                case Boolean:
                case Double:
                case Int32:
                    return value;
                case JsValue js when js is not ObjectInstance:
                    return js.IsUndefined() || js.IsNull() ? null : Clone(js.ToObject(), path, depth);
                case ObjectInstance source when source is not IDomProxy && source is not Function && depth < MaxDepth && path.Add(source):
                    var result = CloneObject(source, path, depth);
                    path.Remove(source);
                    return result;
                default:
                    throw new DomException(DomError.DataClone);
            }
        }

        private static Object CloneObject(ObjectInstance source, HashSet<Object> path, Int32 depth)
        {
            if (source is JsDate date)
            {
                return date.ToObject();
            }

            if (source is ArrayInstance array)
            {
                var items = new Object[(Int32)array.Get("length").AsNumber()];

                for (var i = 0; i < items.Length; i++)
                {
                    items[i] = Clone(array.Get(i), path, depth + 1);
                }

                return items;
            }

            var record = new Dictionary<String, Object>();

            foreach (var key in source.GetOwnPropertyKeys())
            {
                if (key.IsString() && source.GetOwnProperty(key).Enumerable)
                {
                    record[key.AsString()] = Clone(source.Get(key), path, depth + 1);
                }
            }

            return record;
        }
    }
}
