namespace AngleSharp.Js.Cache
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The WebIDL constants of the interfaces (e.g. Node.ELEMENT_NODE), which the libraries
    /// declare as the fields of an enum named like the interface owning them.
    /// </summary>
    static class ConstantCache
    {
        private static readonly ConcurrentDictionary<Assembly, IReadOnlyDictionary<String, KeyValuePair<String, Double>[]>> _constants = new();

        //  Enums whose name doesn't say the interface owning their constants (AngleSharp 1.10 names
        //  the document for those of a node, which all nodes have).
        private static readonly Dictionary<String, String> _owners = new(StringComparer.Ordinal)
        {
            { "AngleSharp.Dom.NodeType", "Node" },
            { "AngleSharp.Dom.DocumentPositions", "Node" },
            { "AngleSharp.Dom.DomError", "DOMException" },
        };

        public static IEnumerable<KeyValuePair<String, Double>> GetConstants(this IEnumerable<Assembly> libs, String name)
        {
            if (name is null)
            {
                return [];
            }

            return libs
                .Select(m => _constants.GetOrAdd(m, Collect))
                .SelectMany(m => m.TryGetValue(name, out var constants) ? constants : []);
        }

        private static IReadOnlyDictionary<String, KeyValuePair<String, Double>[]> Collect(Assembly assembly)
        {
            var result = new Dictionary<String, List<KeyValuePair<String, Double>>>(StringComparer.Ordinal);

            foreach (var type in GetTypes(assembly))
            {
                var info = type.GetTypeInfo();

                if (!info.IsEnum || info.GetCustomAttribute<DomLiteralsAttribute>() != null)
                {
                    continue;
                }

                var owner = _owners.TryGetValue(type.FullName ?? String.Empty, out var known) ? known :
                    info.GetCustomAttributes<DomNameAttribute>().FirstOrDefault()?.OfficialName;

                if (owner is null)
                {
                    continue;
                }

                foreach (var field in info.DeclaredFields.Where(m => m.IsLiteral))
                {
                    var constant = field.GetCustomAttributes<DomNameAttribute>().FirstOrDefault()?.OfficialName;

                    if (constant is not null && IsConstantName(constant))
                    {
                        if (!result.TryGetValue(owner, out var list))
                        {
                            result[owner] = list = [];
                        }

                        if (!list.Any(m => m.Key == constant))
                        {
                            list.Add(new KeyValuePair<String, Double>(constant, Convert.ToDouble(field.GetRawConstantValue())));
                        }
                    }
                }
            }

            return result.ToDictionary(m => m.Key, m => m.Value.ToArray(), StringComparer.Ordinal);
        }

        //  constants are all upper case (the members of a string enumeration are not)
        private static Boolean IsConstantName(String name) =>
            name.Length > 0 && Char.IsUpper(name[0]) && name.All(c => Char.IsUpper(c) || Char.IsDigit(c) || c == '_');

        private static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.ExportedTypes;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
