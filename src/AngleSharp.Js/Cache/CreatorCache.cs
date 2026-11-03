using AngleSharp.Attributes;
using AngleSharp.Js.Attributes;
using AngleSharp.Js.Proxies;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;

namespace AngleSharp.Js.Cache
{
    static class CreatorCache
    {
        private static readonly ConcurrentDictionary<Type, ConstructorDefinition> _constructorDefinitions = new();
        private static readonly ConcurrentDictionary<Type, EnumLiteralDefinition> _enumLiteralDefinitions = new();
        private static readonly ConcurrentDictionary<Assembly, ISet<String>> _nonEnumTypeNames = new();
        private static readonly ConcurrentDictionary<Type, StaticMember[]> _staticMembers = new();
        private static readonly ConcurrentDictionary<Enum, String> _literalNames = new();
        private static readonly ConcurrentDictionary<Type, MethodInfo[]> _implicitConversions = new();

        /// <summary>
        /// Gets the implicit conversion back from a union, i.e. from a type that also converts
        /// from other types, as an object without DOM members would be of no use to a script.
        /// A type only converting away (Url to Uri) is no union and remains what it is.
        /// </summary>
        public static MethodInfo GetGivenConversion(this Type type)
        {
            if (type == typeof(void) || type == typeof(String) || !type.GetTypeInfo().IsClass || type.GetImplicitConversions().Length == 0)
            {
                return null;
            }

            return type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m => m.IsSpecialName && m.Name == "op_Implicit" && m.ReturnType != type && m.GetParameters()[0].ParameterType == type);
        }

        /// <summary>
        /// Gets the implicit conversions a type declares to itself, in the order of declaration.
        /// </summary>
        public static MethodInfo[] GetImplicitConversions(this Type type) => _implicitConversions.GetOrAdd(type, static target =>
            target.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(m => m.IsSpecialName && m.Name == "op_Implicit" && m.ReturnType == target)
                .ToArray());

        /// <summary>
        /// Gets the string script sees for a value of a WebIDL enumeration, or null if the enum
        /// is a set of constants, which stay numbers.
        /// </summary>
        public static String GetLiteralName(this Enum value) => _literalNames.GetOrAdd(value, static member =>
        {
            var type = member.GetType();
            var isLiteral = type == typeof(DocumentReadyState) || type.GetTypeInfo().GetCustomAttribute<DomLiteralsAttribute>() != null;

            return isLiteral
                ? type.GetTypeInfo().GetDeclaredField(member.ToString())?.GetCustomAttribute<DomNameAttribute>()?.OfficialName
                : null;
        });

        /// <summary>
        /// Gets the static members a type exposes on its constructor, the ones of base classes
        /// included. A derived class hides a member of the same name, as a shadowing property
        /// of a constructor would.
        /// </summary>
        public static StaticMember[] GetStaticMembers(this Type type) => _staticMembers.GetOrAdd(type, static target =>
        {
            var members = new Dictionary<String, StaticMember>(StringComparer.Ordinal);
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

            for (var current = target; current != null; current = current.GetTypeInfo().BaseType)
            {
                var found = new Dictionary<String, StaticMember>(StringComparer.Ordinal);

                foreach (var method in current.GetMethods(flags))
                {
                    //  Extending the window is how a namespace such as CSS gets its window in C#;
                    //  anything else extended is no member of this type.
                    if (method.IsSpecialName || (method.IsDefined(typeof(ExtensionAttribute), false) && !MethodDescription.Of(method).TakesWindow))
                    {
                        continue;
                    }

                    var accessor = method.GetCustomAttribute<DomAccessorAttribute>()?.Type;

                    foreach (var attribute in method.GetCustomAttributes<DomNameAttribute>())
                    {
                        if (!found.TryGetValue(attribute.OfficialName, out var member))
                        {
                            member = new StaticMember(attribute.OfficialName);
                            found.Add(attribute.OfficialName, member);
                        }

                        if (accessor == Accessors.Getter)
                        {
                            member.Getter = method;
                        }
                        else if (accessor == Accessors.Setter)
                        {
                            member.Setter = method;
                        }
                        else
                        {
                            member.Add(method);
                        }
                    }
                }

                foreach (var property in current.GetProperties(flags))
                {
                    foreach (var attribute in property.GetCustomAttributes<DomNameAttribute>())
                    {
                        found[attribute.OfficialName] = new StaticMember(attribute.OfficialName)
                        {
                            Getter = property.GetMethod,
                            Setter = property.SetMethod,
                        };
                    }
                }

                foreach (var entry in found)
                {
                    if (!members.ContainsKey(entry.Key))
                    {
                        members.Add(entry.Key, entry.Value);
                    }
                }
            }

            return members.Values.ToArray();
        });

        /// <summary>
        /// Gets what is needed to build the constructor object for a type, or null if the
        /// type is not exposed as one. The answer depends on the type alone, so the null
        /// is cached as well - most exported types do not get a constructor.
        /// </summary>
        public static ConstructorDefinition GetConstructorDefinition(this Type type)
        {
            if (!_constructorDefinitions.TryGetValue(type, out var definition))
            {
                var ti = type.GetTypeInfo();
                var names = ti.GetCustomAttributes<DomNameAttribute>()
                    .Select(m => m.OfficialName)
                    .Where(m => m != null)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                if (names.Length > 0 && !ti.IsEnum)
                {
                    var overloads = ti.DeclaredConstructors.Where(m => m.GetCustomAttributes<DomConstructorAttribute>().Any()).ToArray();
                    definition = new ConstructorDefinition(type, names, overloads);
                }

                _constructorDefinitions.TryAdd(type, definition);
            }

            return definition;
        }

        /// <summary>
        /// Gets what is needed to publish an enum as a standalone DOM literal object,
        /// or null if the enum should not be exposed that way.
        /// </summary>
        public static EnumLiteralDefinition GetEnumLiteralDefinition(this Type type)
        {
            if (!_enumLiteralDefinitions.TryGetValue(type, out var definition))
            {
                var ti = type.GetTypeInfo();

                //  A WebIDL enumeration is a set of strings without an object of its own.
                if (ti.IsEnum && ti.GetCustomAttribute<DomLiteralsAttribute>() == null)
                {
                    var name = ti.GetCustomAttributes<DomNameAttribute>(true).FirstOrDefault()?.OfficialName;

                    if (name != null)
                    {
                        var typeNames = GetNonEnumTypeNames(type.GetAssembly());

                        if (!typeNames.Contains(name))
                        {
                            var members = ti.DeclaredFields
                                .Where(m => m.IsLiteral)
                                .Select(m => new EnumLiteralMember(
                                    m.GetCustomAttributes<DomNameAttribute>().FirstOrDefault()?.OfficialName,
                                    m.GetRawConstantValue()))
                                .Where(m => m.Name != null)
                                .ToArray();

                            if (members.Length > 0)
                            {
                                definition = new EnumLiteralDefinition(name, members);
                            }
                        }
                    }
                }

                _enumLiteralDefinitions.TryAdd(type, definition);
            }

            return definition;
        }

        private static ISet<String> GetNonEnumTypeNames(Assembly assembly)
        {
            if (!_nonEnumTypeNames.TryGetValue(assembly, out var names))
            {
                names = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

                foreach (var exportedType in assembly.ExportedTypes)
                {
                    var ti = exportedType.GetTypeInfo();

                    if (ti.IsEnum)
                    {
                        continue;
                    }

                    var name = ti.GetCustomAttributes<DomNameAttribute>(true).FirstOrDefault()?.OfficialName;

                    if (name != null)
                    {
                        names.Add(name);
                    }
                }

                _nonEnumTypeNames.TryAdd(assembly, names);
            }

            return names;
        }

        private static readonly ConcurrentDictionary<Type, Action<EngineInstance, ObjectInstance>> _constructorFunctionActions = new();

        public static Action<EngineInstance, ObjectInstance> GetConstructorFunctionAction(this Type type)
        {
            if (!_constructorFunctionActions.TryGetValue(type, out var action))
            {
                var constructorFunctions = type.GetTypeInfo().GetMethods().Where(m => m.GetCustomAttributes<DomConstructorFunctionAttribute>().Any());

                if (constructorFunctions.Any())
                {
                    action = (engine, obj) =>
                    {
                        foreach (var constructorFunction in constructorFunctions)
                        {
                            var attribute = constructorFunction.GetCustomAttribute<DomConstructorFunctionAttribute>();

                            var constructorFunctionInstance = new DomConstructorFunctionInstance(engine, constructorFunction, attribute.OfficialName);

                            obj.FastSetProperty(attribute.OfficialName, new PropertyDescriptor(constructorFunctionInstance, false, true, false));
                        }
                    };
                }
                else
                {
                    action = (e, o) => { };
                }

                _constructorFunctionActions.TryAdd(type, action);
            }

            return action;
        }

        private static readonly ConcurrentDictionary<Type, Action<EngineInstance, ObjectInstance>> _instanceActions = new();

        public static Action<EngineInstance, ObjectInstance> GetInstanceAction(this Type type)
        {
            if (!_instanceActions.TryGetValue(type, out var action))
            {
                var info = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(m => m.GetParameters().Length == 0);

                if (info != null)
                {
                    var attributes = type.GetTypeInfo().GetCustomAttributes<DomInstanceAttribute>();
                    action = (engine, obj) =>
                    {
                        foreach (var attribute in attributes)
                        {
                            var instance = info.Invoke(null);

                            if (instance != null)
                            {
                                var node = engine.GetDomNode(instance);
                                obj.FastSetProperty(attribute.Name, new PropertyDescriptor(node, false, true, false));
                            }
                        }
                    };
                }
                else
                {
                    action = (e, o) => { };
                }

                _instanceActions.TryAdd(type, action);
            }

            return action;
        }
    }

    sealed class EnumLiteralDefinition
    {
        public EnumLiteralDefinition(String name, EnumLiteralMember[] members)
        {
            Name = name;
            Members = members;
        }

        public String Name { get; }

        public EnumLiteralMember[] Members { get; }
    }

    readonly struct EnumLiteralMember
    {
        public EnumLiteralMember(String name, Object value)
        {
            Name = name;
            Value = value;
        }

        public String Name { get; }

        public Object Value { get; }
    }

    /// <summary>
    /// Everything the constructor object of a type is built from. The reflection behind it
    /// is the same for every engine, so it is resolved once and kept by
    /// <see cref="CreatorCache"/> - only the object built from it belongs to an engine.
    /// </summary>
    sealed class ConstructorDefinition
    {
        public ConstructorDefinition(Type type, String[] names, ConstructorInfo[] overloads)
        {
            Type = type;
            Names = names;
            Overloads = overloads;
        }

        /// <summary>
        /// Gets the type the constructor creates instances of.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the names the constructor is exposed under.
        /// </summary>
        public String[] Names { get; }

        /// <summary>
        /// Gets the primary name of the constructor.
        /// </summary>
        public String Name => Names[0];

        /// <summary>
        /// Gets the constructors to pick from, which are none if the type cannot be constructed
        /// from script - naming it is still legal, calling it is not.
        /// </summary>
        public ConstructorInfo[] Overloads { get; }
    }

    /// <summary>
    /// Describes a static member of a DOM type, which belongs on the constructor object.
    /// </summary>
    sealed class StaticMember
    {
        private readonly List<MethodInfo> _overloads = new List<MethodInfo>();

        public StaticMember(String name)
        {
            Name = name;
        }

        public String Name { get; }

        public MethodInfo Getter { get; set; }

        public MethodInfo Setter { get; set; }

        public Boolean IsAccessor => Getter != null || Setter != null;

        public MethodInfo[] Overloads => _overloads.ToArray();

        public void Add(MethodInfo method) => _overloads.Add(method);
    }
}
