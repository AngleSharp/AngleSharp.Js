namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    static class ReflectionExtensions
    {
        public static String[] GetParameterNames(this MethodInfo method) =>
            method?.GetParameters().Select(m => m.Name).ToArray();

        public static Assembly GetAssembly(this Type type) => type.GetTypeInfo().Assembly;

        public static Object GetDefaultValue(this Type type) =>
            type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;

        public static MethodInfo PrepareConvert(this Type fromType, Type toType)
        {
            try
            {
                // Throws an exception if there is no conversion from fromType to toType
                var exp = Expression.Convert(Expression.Parameter(fromType, null), toType);
                return exp.Method;
            }
            catch
            {
                return null;
            }
        }

        public static String GetOfficialName(this MemberInfo member)
        {
            var names = member.GetCustomAttributes<DomNameAttribute>();
            var officalNameAttribute = names.FirstOrDefault();
            return officalNameAttribute?.OfficialName ?? member.Name;
        }

        public static String GetOfficialName(this Type currentType, Type baseType)
        {
            var ti = currentType.GetTypeInfo();
            var name = ti.GetCustomAttribute<DomNameAttribute>(true)?.OfficialName;

            if (name == null)
            {
                var interfaces = ti.ImplementedInterfaces;

                if (baseType != null)
                {
                    var bi = baseType.GetTypeInfo();
                    var exclude = bi.ImplementedInterfaces;
                    interfaces = interfaces.Except(exclude);
                }

                foreach (var impl in interfaces)
                {
                    name = impl.GetTypeInfo().GetCustomAttribute<DomNameAttribute>(false)?.OfficialName;

                    if (name != null)
                        break;
                }
            }

            return name;
        }

        public static PropertyInfo GetInheritedProperty(this Type type, String propertyName, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            if (!type.IsInterface)
            {
                return type.GetProperty(propertyName, bindingAttr);
            }

            return type.GetInterfaces()
                .Union(new [] { type })
                .Select(i => i.GetProperty(propertyName, bindingAttr))
                .Where(propertyInfo => propertyInfo != null)
                .FirstOrDefault();
        }

        public static IEnumerable<PropertyInfo> GetInheritedProperties(this Type type, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            if (!type.IsInterface)
            {
                return type.GetProperties(bindingAttr);
            }

            return type.GetInterfaces()
                .Union(new [] { type })
                .SelectMany(i => i.GetProperties(bindingAttr))
                .Distinct();
        }
    }
}