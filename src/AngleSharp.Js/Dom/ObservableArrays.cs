namespace AngleSharp.Js
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime.Interop;
    using Jint.Runtime;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// WebIDL's ObservableArray (e.g. adoptedStyleSheets), a mutable IList&lt;T&gt; in the
    /// DOM: script sees a real array (concat, spread, push, ...), whose changes go to the list.
    /// The array is the same object each time; it shows what the list has when it is got.
    /// </summary>
    sealed class ObservableArrays
    {
        private const String ProxyFactory = @"(function (target, sync) {
            return new Proxy(target, {
                set: function (t, key, value) { t[key] = value; sync(t); return true; },
                deleteProperty: function (t, key) { delete t[key]; sync(t); return true; }
            });
        })";

        private readonly EngineInstance _engine;
        private readonly ConditionalWeakTable<Object, Entry> _arrays = new ConditionalWeakTable<Object, Entry>();
        private JsValue _factory;

        public ObservableArrays(EngineInstance engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Gets the element type if the declared type stands for an ObservableArray, else null.
        /// </summary>
        public static Type GetElementType(Type type) =>
            type != null && type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>)
                ? type.GetGenericArguments()[0]
                : null;

        public JsValue Get(Object list, Type elementType)
        {
            var entry = _arrays.GetValue(list, key => Create(key, elementType));
            var items = (IEnumerable)list;
            var values = new List<JsValue>();

            foreach (var item in items)
            {
                values.Add(item.ToJsValue(_engine));
            }

            entry.Syncing = true;

            try
            {
                entry.Target.Set("length", 0);

                for (var i = 0; i < values.Count; i++)
                {
                    entry.Target.Set(i, values[i]);
                }
            }
            finally
            {
                entry.Syncing = false;
            }

            return entry.Proxy;
        }

        private Entry Create(Object list, Type elementType)
        {
            var jint = _engine.Jint;
            _factory ??= jint.Evaluate(ProxyFactory);
            var entry = new Entry { Target = new JsArray(jint) };
            var listType = typeof(ICollection<>).MakeGenericType(elementType);
            var clear = listType.GetMethod("Clear");
            var add = listType.GetMethod("Add");
            var sync = new ClrFunction(jint, "sync", (thisObject, arguments) =>
            {
                if (!entry.Syncing)
                {
                    var length = (Int32)TypeConverter.ToNumber(entry.Target.Get("length"));
                    var items = new List<Object>(length);

                    for (var i = 0; i < length; i++)
                    {
                        var value = entry.Target.Get(i);

                        //  a hole is what an array method leaves on its way (pop deletes, then shortens)
                        if (value.IsUndefined())
                        {
                            continue;
                        }

                        var item = value.FromJsValue();

                        if (item == null || !elementType.IsInstanceOfType(item))
                        {
                            throw new JavaScriptException(jint.Intrinsics.TypeError, "The value is not of the expected type.");
                        }

                        items.Add(item);
                    }

                    clear.Invoke(list, null);

                    foreach (var item in items)
                    {
                        add.Invoke(list, new[] { item });
                    }
                }

                return JsValue.Undefined;
            });
            entry.Proxy = jint.Invoke(_factory, entry.Target, sync);
            return entry;
        }

        private sealed class Entry
        {
            public JsArray Target;
            public JsValue Proxy;
            public Boolean Syncing;
        }
    }
}
