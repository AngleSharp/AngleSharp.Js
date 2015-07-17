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
                var binding = new BindingClass(name);
                binding.AttachProperties(type.GetProperties());
                binding.AttachEvents(type.GetEvents());
                binding.AttachMethods(type.GetMethods());
                binding.AttachConstructors(type.GetConstructors());
                return binding;
            });
        }
    }
}
