namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System.Collections.Generic;
    using System.Reflection;

    static class AttachExtensions
    {
        public static void AttachAll(this BindingClass target, IEnumerable<DomNameAttribute> nameAttributes, BindingMember member)
        {
            foreach (var nameAttribute in nameAttributes)
                target.Bind(nameAttribute.OfficialName, member);
        }

        public static void AttachAll(this BindingClass target, DomAccessorAttribute accessorAttribute, BindingMember member)
        {
            foreach (var accessor in accessorAttribute.GetAccessors())
                target.Bind(accessor, member);
        }

        public static void AttachAll(this BindingClass target, DomConstructorAttribute ctorAttribute, BindingConstructor constructor)
        {
            if (ctorAttribute != null)
                target.BindConstructor(constructor);
        }

        public static void AttachFields(this BindingClass target, IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields)
            {
                var nameAttributes = field.GetDomNameAttributes();
                var binding = new BindingField(field.Name, field.FieldType);

                target.AttachAll(nameAttributes, binding);
            }
        }

        public static void AttachMethods(this BindingClass target, IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (method.IsConstructor)
                    continue;

                var nameAttributes = method.GetDomNameAttributes();
                var access = method.GetDomAccessorAttribute();
                var binding = method.CreateMethodBinding();

                target.AttachAll(nameAttributes, binding);
                target.AttachAll(access, binding);
            }
        }

        public static void AttachConstructors(this BindingClass target, IEnumerable<ConstructorInfo> constructors)
        {
            foreach (var constructor in constructors)
            {
                var ctorAttribute = constructor.GetDomConstructorAttribute();
                var binding = constructor.CreateConstructorBinding();

                target.AttachAll(ctorAttribute, binding);
            }
        }

        public static void AttachProperties(this BindingClass target, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var nameAttributes = property.GetDomNameAttributes();
                var access = property.GetDomAccessorAttribute();
                var binding = property.CreatePropertyOrIndexBinding();

                target.AttachAll(nameAttributes, binding);
                target.AttachAll(access, binding);
            }
        }

        public static void AttachEvents(this BindingClass target, IEnumerable<EventInfo> events)
        {
            foreach (var evt in events)
            {
                var nameAttributes = evt.GetDomNameAttributes();
                var binding = evt.CreateEventBinding();

                target.AttachAll(nameAttributes, binding);
            }
        }
    }
}
