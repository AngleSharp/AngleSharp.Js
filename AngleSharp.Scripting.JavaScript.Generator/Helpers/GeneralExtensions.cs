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
            var names = type.GetEnumNames();
            var values = type.GetEnumValues();

            for (int i = 0; i < names.Length; i++)
            {
                var key = names[i];
                var val = values.GetValue(i);
                binding.Bind(key, val);
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
