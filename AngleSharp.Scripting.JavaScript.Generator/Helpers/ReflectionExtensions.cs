namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class ReflectionExtensions
    {
        public static IEnumerable<Accessors> GetAccessors(this DomAccessorAttribute accessorAttribute)
        {
            if (accessorAttribute != null)
            {
                var values = Enum.GetValues(typeof(Accessors)) as Accessors[];
                return values.Where(m => m != Accessors.None && accessorAttribute.Type.HasFlag(m));
            }

            return Enumerable.Empty<Accessors>();
        }

        public static DomNoInterfaceObjectAttribute GetDomNoInterfaceObjectAttribute(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomNoInterfaceObjectAttribute>(inherit: false);
        }

        public static DomAccessorAttribute GetDomAccessorAttribute(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomAccessorAttribute>(inherit: false);
        }

        public static DomConstructorAttribute GetDomConstructorAttribute(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomConstructorAttribute>(inherit: false);
        }

        public static DomLenientThisAttribute GetDomLenientThisAttribute(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomLenientThisAttribute>(inherit: false);
        }

        public static DomPutForwardsAttribute GetDomPutForwardsAttribute(this MemberInfo member)
        {
            return member.GetCustomAttribute<DomPutForwardsAttribute>(inherit: false);
        }

        public static IEnumerable<DomNameAttribute> GetDomNameAttributes(this MemberInfo member)
        {
            return member.GetCustomAttributes<DomNameAttribute>(inherit: false);
        }

        public static Dictionary<String, Type> GetCandidates(this Assembly assembly)
        {
            var candidates = new Dictionary<String, Type>();

            foreach (var type in assembly.ExportedTypes)
            {
                var nameAttributes = type.GetDomNameAttributes();

                foreach (var nameAttribute in nameAttributes)
                {
                    var name = nameAttribute.OfficialName;
                    candidates.Add(name, type);
                }
            }

            return candidates;
        }

        public static Dictionary<String, Type> GetParameterMap(this MethodBase method)
        {
            return method.GetParameters().Map();
        }

        public static Dictionary<String, Type> Map(this IEnumerable<ParameterInfo> parameters)
        {
            return parameters.ToDictionary(m => m.Name, m => m.ParameterType);
        }
    }
}
