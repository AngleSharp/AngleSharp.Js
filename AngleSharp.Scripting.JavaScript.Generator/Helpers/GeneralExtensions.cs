namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

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
                else
                    yield return GetClassBinding(name, type);
            }
        }

        static BindingEnum GetEnumBinding(String name, Type type)
        {
            var binding = new BindingEnum(name);
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
            var noInterfaceObject = type.GetDomNoInterfaceObjectAttribute() != null;
            var binding = new BindingClass(name, noInterfaceObject);
            binding.AttachProperties(type.GetProperties());
            binding.AttachEvents(type.GetEvents());
            binding.AttachMethods(type.GetMethods());
            binding.AttachConstructors(type.GetConstructors());
            return binding;
        }
    }
}
