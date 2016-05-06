namespace AngleSharp.Scripting.CSharp
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class InternalHelpers
    {
        public static Dictionary<String, MemberInfo> CreateMemberMap(this Type type)
        {
            var members = new Dictionary<String, MemberInfo>();
            var interfaces = type.GetTypeInfo().GetAllImplementedInterfaces().Distinct().ToArray();

            foreach (var method in type.GetRuntimeMethods())
            {
                var attr = method.GetCustomAttribute<DomNameAttribute>(true);

                if (attr == null)
                {
                    attr = interfaces.SelectMany(m => m.DeclaredMethods).
                                      Where(m => m.SameAs(method)).
                                      SelectMany(m => m.GetCustomAttributes<DomNameAttribute>(true)).
                                      FirstOrDefault(m => m != null);
                }

                if (attr != null && members.ContainsKey(attr.OfficialName) == false)
                    members.Add(attr.OfficialName, method);
            }

            foreach (var property in type.GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<DomNameAttribute>(true);

                if (attr == null)
                {
                    attr = interfaces.SelectMany(m => m.DeclaredProperties).
                                      Where(m => m.SameAs(property)).
                                      SelectMany(m => m.GetCustomAttributes<DomNameAttribute>(true)).
                                      FirstOrDefault(m => m != null);
                }

                if (attr != null && members.ContainsKey(attr.OfficialName) == false)
                    members.Add(attr.OfficialName, property);
            }

            return members;
        }

        public static IEnumerable<TypeInfo> GetAllImplementedInterfaces(this TypeInfo type)
        {
            var topifs = type.ImplementedInterfaces.Select(m => m.GetTypeInfo()).Where(m => m.IsPublic);

            foreach (var topif in topifs)
            {
                var subifs = topif.GetAllImplementedInterfaces();

                foreach (var subif in subifs)
                    yield return subif;

                yield return topif;
            }
        }

        public static Boolean IsCustom(this Object obj)
        {
            return obj != null && obj.GetType().GetTypeInfo().Assembly == typeof(BrowsingContext).GetTypeInfo().Assembly;
        }

        public static Boolean SameAs(this PropertyInfo a, PropertyInfo b)
        {
            return a.Name == b.Name && a.PropertyType == b.PropertyType;
        }

        public static Boolean SameAs(this MethodInfo a, MethodInfo b)
        {
            if (a.Name == b.Name && a.ReturnType == b.ReturnType)
            {
                var pa = a.GetParameters();
                var pb = b.GetParameters();

                if (pa.Length == pb.Length)
                {
                    for (int i = 0; i < pa.Length; i++)
                    {
                        if (pa[i].ParameterType != pb[i].ParameterType)
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
