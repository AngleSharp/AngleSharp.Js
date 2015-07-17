namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class BindingExtensions
    {
        static BindingIndex CreateIndexBinding(String name, Accessors access, PropertyInfo property)
        {
            var binding = new BindingIndex(name);
            return binding;
        }

        static BindingProperty CreatePropertyBinding(String name, PropertyInfo property)
        {
            var binding = new BindingProperty(name);
            var lenient = property.GetDomLenientThisAttribute();
            var putsForward = property.GetDomPutForwardsAttribute();
            binding.OriginalName = property.Name;
            binding.AllowGet = property.CanRead;
            binding.AllowSet = property.CanWrite;
            binding.IsLenient = lenient != null;

            if (putsForward != null)
                binding.ForwardedTo = putsForward.PropertyName;

            return binding;
        }

        public static void AttachEvents(this BindingClass target, IEnumerable<EventInfo> events)
        {
            foreach (var evt in events)
            {
                var nameAttributes = evt.GetDomNameAttributes();
            }
        }

        public static void AttachMethods(this BindingClass target, IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var nameAttributes = method.GetDomNameAttributes();
                var access = method.GetDomAccessorAttribute();
            }
        }

        public static void AttachConstructors(this BindingClass target, IEnumerable<ConstructorInfo> constructors)
        {
            foreach (var constructor in constructors)
            {
                var nameAttributes = constructor.DeclaringType.GetDomNameAttributes();
                var ctorAttribute = constructor.GetDomConstructorAttribute();
            }
        }

        public static void AttachProperties(this BindingClass target, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var nameAttributes = property.GetDomNameAttributes();
                var access = property.GetDomAccessorAttribute();
                var binding = default(BindingMember);

                foreach (var nameAttribute in nameAttributes)
                {
                    var name = nameAttribute.OfficialName;

                    if (access == null)
                        binding = CreatePropertyBinding(name, property);
                    else
                        binding = CreateIndexBinding(name, access.Type, property);

                    target.Bind(name, binding);
                }
            }
        }
    }
}
