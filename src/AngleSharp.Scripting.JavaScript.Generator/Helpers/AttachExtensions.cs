namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class AttachExtensions
    {
        public static void AttachAll(this BindingClass target, IEnumerable<String> names, BindingMember member)
        {
            foreach (var name in names)
                target.Bind(name, member);
        }

        public static void AttachAll(this BindingClass target, IEnumerable<Accessors> accessors, BindingFunction member)
        {
            foreach (var accessor in accessors)
                target.Bind(accessor, member);
        }

        public static void AttachFields(this BindingClass target, IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields)
            {
                var names = field.GetDomNames();
                var binding = new BindingField(field.Name, field.FieldType);

                target.AttachAll(names, binding);
            }
        }

        public static void AttachMethods(this BindingClass target, IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (method.IsConstructor)
                    continue;

                var names = method.GetDomNames();
                var accessors = method.GetDomAccessors();
                var binding = method.CreateMethodBinding();

                target.AttachAll(names, binding);
                target.AttachAll(accessors, binding);
            }
        }

        public static void AttachConstructors(this BindingClass target, IEnumerable<ConstructorInfo> constructors)
        {
            foreach (var constructor in constructors)
            {
                if (constructor.IsConstructorExposed())
                {
                    var binding = constructor.CreateConstructorBinding();
                    target.BindConstructor(binding);
                }
            }
        }

        public static void AttachProperties(this BindingClass target, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var names = property.GetDomNames();
                var accessors = property.GetDomAccessors();
                var binding = property.CreatePropertyOrIndexBinding();

                target.AttachAll(names, binding);

                if (binding is BindingFunction)
                    target.AttachAll(accessors, binding as BindingFunction);
            }
        }

        public static void AttachEvents(this BindingClass target, IEnumerable<EventInfo> events)
        {
            foreach (var evt in events)
            {
                var names = evt.GetDomNames();
                var binding = evt.CreateEventBinding();

                target.AttachAll(names, binding);
            }
        }
    }
}
