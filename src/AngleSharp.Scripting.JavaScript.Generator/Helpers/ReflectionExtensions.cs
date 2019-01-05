namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class ReflectionExtensions
    {
        public static Boolean IsNotInterfaced(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomNoInterfaceObjectAttribute>(inherit: false) != null;
        }

        public static Boolean IsParams(this ParameterInfo parameter)
        {
            return parameter.GetCustomAttributes<ParamArrayAttribute>(inherit: false).Any();
        }

        public static IEnumerable<Accessors> GetDomAccessors(this MemberInfo member)
        {
            var attr = member.GetCustomAttribute<DomAccessorAttribute>(inherit: false);

            if (attr != null)
            {
                var values = Enum.GetValues(typeof(Accessors)) as Accessors[];
                return values.Where(m => m != Accessors.None && attr.Type.HasFlag(m));
            }

            return Enumerable.Empty<Accessors>();
        }

        public static Boolean IsConstructorExposed(this ConstructorInfo member)
        {
            return member.GetCustomAttribute<DomConstructorAttribute>(inherit: false) != null;
        }

        public static Boolean HasLenientThis(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomLenientThisAttribute>(inherit: false) != null;
        }

        public static String GetPutForwardsTo(this PropertyInfo member)
        {
            var attr = member.GetCustomAttribute<DomPutForwardsAttribute>(inherit: false);

            if (attr != null)
            {
                var target = member.PropertyType.GetProperties().
                                    Where(m => m.GetDomNames().Contains(attr.PropertyName)).
                                    FirstOrDefault();

                if (target != null)
                    return target.Name;
            }

            return null;
        }

        public static IEnumerable<String> GetDomNames(this MemberInfo member)
        {
            return member.GetCustomAttributes<DomNameAttribute>(inherit: false).Select(m => m.OfficialName.Trim());
        }

        public static Dictionary<String, List<Type>> GetCandidates(this Assembly assembly)
        {
            var candidates = new Dictionary<String, List<Type>>();

            foreach (var type in assembly.ExportedTypes)
            {
                // Delegates are not interesting
                if (typeof(Delegate).IsAssignableFrom(type))
                    continue;

                var names = type.GetDomNames().Select(m => m.Capitalize());

                foreach (var name in names)
                {
                    var list = default(List<Type>);

                    if (candidates.TryGetValue(name, out list) == false)
                        candidates.Add(name, list = new List<Type>());

                    list.Add(type);
                }
            }

            return candidates;
        }

        public static IEnumerable<T> GetAll<T>(this Type type, Func<Type, IEnumerable<T>> selectFrom)
        {
            var members = selectFrom(type);
            var interfaces = type.GetExclusiveInterfaces();
            var others = interfaces.SelectMany(m => m.GetAll(selectFrom));
            return members.Concat(others);
        }

        public static IEnumerable<Type> GetExclusiveInterfaces(this Type type)
        {
            var interfaces = type.GetInterfaces();
            var allInterfaces = interfaces.Where(m => m.IsNotInterfaced());
            var sharedInterfaces = interfaces.Where(m => !m.IsNotInterfaced()).SelectMany(m => m.GetExclusiveInterfaces());
            return allInterfaces.Except(sharedInterfaces);
        }
    }
}
