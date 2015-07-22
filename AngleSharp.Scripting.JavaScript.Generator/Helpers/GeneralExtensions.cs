namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class GeneralExtensions
    {
        public static IEnumerable<BindingType> GetBindings(this IDictionary<String, Type> mappings)
        {
            foreach (var mapping in mappings)
            {
                var name = mapping.Key;
                var type = mapping.Value;

                if (type.IsEnum)
                    yield return GetEnumBinding(name, type);
                else if (type.GetDomNoInterfaceObjectAttribute() == null)
                    yield return GetClassBinding(name, type);
            }
        }

        static BindingEnum GetEnumBinding(String name, Type type)
        {
            var binding = new BindingEnum(name, type.Name, type.Namespace);
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                var nameAttributes = field.GetDomNameAttributes();
                var enumValue = new BindingField(field.Name, field.FieldType);

                foreach (var nameAttribute in nameAttributes)
                    binding.Bind(nameAttribute.OfficialName, enumValue);
            }

            return binding;
        }
        
        static BindingClass GetClassBinding(String name, Type type)
        {
            var binding = new BindingClass(name, type.Name, type.Namespace, ResolveBase(type));
            binding.AttachProperties(type.GetProperties());
            binding.AttachEvents(type.GetEvents());
            binding.AttachMethods(type.GetMethods());
            binding.AttachConstructors(type.GetConstructors());
            return binding;
        }

        static String ResolveBase(Type type)
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
