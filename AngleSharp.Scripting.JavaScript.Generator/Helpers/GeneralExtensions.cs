namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class GeneralExtensions
    {
        public static IEnumerable<BindingClass> GetBindings(this IDictionary<String, Type> mappings)
        {
            var classes = new List<BindingClass>();

            foreach (var mapping in mappings)
            {
                var name = mapping.Key;
                var type = mapping.Value;

                if (type.GetDomNoInterfaceObjectAttribute() == null)
                    classes.Add(type.GetClassBinding(name, classes));
            }

            return classes;
        }
        
        static BindingClass GetClassBinding(this Type type, String name, IEnumerable<BindingClass> classes)
        {
            var binding = classes.CreateBinding(name, type);
            binding.AttachProperties(type.GetProperties());
            binding.AttachEvents(type.GetEvents());
            binding.AttachMethods(type.GetMethods());
            binding.AttachFields(type.GetFields());
            binding.AttachConstructors(type.GetConstructors());
            return binding;
        }

        static BindingClass CreateBinding(this IEnumerable<BindingClass> reservoir, String name, Type type)
        {
            return reservoir.Where(m => m.Name == name).SingleOrDefault() ?? 
                new BindingClass(name, type.Name, type.Namespace, type.ResolveBase());
        }

        static String ResolveBase(this Type type)
        {
            var name = GetDomNameOrNull(type.BaseType);

            if (name == null)
            {
                var interfaces = type.GetInterfaces();

                for (int i = 0; i < interfaces.Length; i++)
                {
                    name = GetDomNameOrNull(interfaces[i]);

                    if (name != null)
                        return name;
                }

                return "Object";
            }

            return name;
        }

        static String GetDomNameOrNull(Type type)
        {
            return type != null && type.GetDomNoInterfaceObjectAttribute() == null && type.GetDomNameAttributes().Any() ? type.GetDomNameAttributes().First().OfficialName : null;
        }
    }
}
