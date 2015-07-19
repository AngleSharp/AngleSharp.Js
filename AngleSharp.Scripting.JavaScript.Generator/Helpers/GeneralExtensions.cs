namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class GeneralExtensions
    {
        public static IEnumerable<BindingClass> GetBindings(this IDictionary<String, Type> mapping)
        {
            return mapping.Select(m =>
            {
                var name = m.Key;
                var type = m.Value;
                var noInterfaceObject = type.GetDomNoInterfaceObjectAttribute() != null;
                var binding = new BindingClass(name, noInterfaceObject);
                binding.AttachProperties(type.GetProperties());
                binding.AttachEvents(type.GetEvents());
                binding.AttachMethods(type.GetMethods());
                binding.AttachConstructors(type.GetConstructors());
                return binding;
            });
        }
    }
}
