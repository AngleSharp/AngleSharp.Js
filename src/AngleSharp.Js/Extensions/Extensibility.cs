namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class Extensibility
    {
        public static IDictionary<String, ExtensionEntry> GetExtensions(this IEnumerable<MethodInfo> methods)
        {
            var entries = new Dictionary<String, ExtensionEntry>();

            foreach (var method in methods)
            {
                var names = method.GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName);
                var accessors = method.GetCustomAttributes<DomAccessorAttribute>()
                    .Select(m => m.Type);
                var isEvent = method.GetCustomAttribute<DomEventAttribute>() != null;
                var forward = method.GetCustomAttribute<DomPutForwardsAttribute>();

                foreach (var name in names)
                {
                    if (!entries.TryGetValue(name, out var entry))
                    {
                        entry = new ExtensionEntry
                        {
                            Forward = forward,
                        };
                        entries.Add(name, entry);
                    }

                    if (accessors.Any())
                    {
                        var accessor = accessors.FirstOrDefault();

                        if (isEvent)
                        {
                            switch (accessor)
                            {
                                case Accessors.Deleter:
                                    entry.Remover = method;
                                    break;
                                case Accessors.Setter:
                                    entry.Adder = method;
                                    break;
                            }
                        }
                        else
                        {
                            switch (accessor)
                            {
                                case Accessors.Setter:
                                    entry.Setter = method;
                                    break;
                                case Accessors.Getter:
                                    entry.Getter = method;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        entry.Other = method;
                    }
                }
            }

            return entries;
        }

        public class ExtensionEntry
        {
            public MethodInfo Getter;
            public MethodInfo Setter;
            public MethodInfo Adder;
            public MethodInfo Remover;
            public MethodInfo Other;
            public DomPutForwardsAttribute Forward;
        }
    }
}
