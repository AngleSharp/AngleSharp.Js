namespace AngleSharp.Js
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    static class ReflectionExtensions
    {
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
