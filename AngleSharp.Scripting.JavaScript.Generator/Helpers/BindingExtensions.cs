namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System.Collections.Generic;
    using System.Reflection;

    static class BindingExtensions
    {
        static BindingIndex CreateIndexBinding(ParameterInfo[] parameters, PropertyInfo propertyInfo)
        {
            var lenient = propertyInfo.GetDomLenientThisAttribute();
            return new BindingIndex(
                originalName: "this",
                canRead: propertyInfo.CanRead,
                canWrite: propertyInfo.CanWrite,
                isLenient: lenient != null,
                valueType: propertyInfo.PropertyType,
                parameters: parameters.Map());
        }

        static BindingMember CreatePropertyBinding(PropertyInfo propertyInfo)
        {
            var parameters = propertyInfo.GetIndexParameters();

            if (parameters.Length == 0)
            {
                var lenient = propertyInfo.GetDomLenientThisAttribute();
                var putsForward = propertyInfo.GetDomPutForwardsAttribute();
                return new BindingProperty(
                    originalName: propertyInfo.Name,
                    canRead: propertyInfo.CanRead,
                    canWrite: propertyInfo.CanWrite,
                    isLenient: lenient != null,
                    forwardedTo: putsForward != null ? putsForward.PropertyName : null,
                    valueType: propertyInfo.PropertyType);
            }

            return CreateIndexBinding(parameters, propertyInfo);
        }

        static BindingConstructor CreateConstructorBinding(ConstructorInfo constructorInfo)
        {
            return new BindingConstructor(
                originalName: constructorInfo.DeclaringType.Name,
                parameters: constructorInfo.GetParameterMap());
        }

        static BindingEvent CreateEventBinding(EventInfo eventInfo)
        {
            var lenient = eventInfo.GetDomLenientThisAttribute();
            return new BindingEvent(
                originalName: eventInfo.Name,
                handlerType: eventInfo.EventHandlerType,
                isLenient: lenient != null);
        }

        static BindingMethod CreateMethodBinding(MethodInfo methodInfo)
        {
            return new BindingMethod(
                originalName: methodInfo.Name,
                parameters: methodInfo.GetParameterMap(),
                returnType: methodInfo.ReturnType);
        }

        public static void AttachAll(this BindingClass target, IEnumerable<DomNameAttribute> nameAttributes, BindingMember member)
        {
            foreach (var nameAttribute in nameAttributes)
                target.Bind(nameAttribute.OfficialName, member);

            target.Wrap(member);
        }

        public static void AttachAll(this BindingClass target, DomAccessorAttribute accessorAttribute, BindingMember member)
        {
            var accessors = accessorAttribute.Type.Split();

            foreach (var accessor in accessors)
                target.Bind(accessor, member);
        }

        public static void AttachAll(this BindingClass target, DomConstructorAttribute ctorAttribute, BindingMember member)
        {
            if (ctorAttribute != null)
                target.BindConstructor(member);

            target.Wrap(member);
        }

        public static void AttachEvents(this BindingClass target, IEnumerable<EventInfo> events)
        {
            foreach (var evt in events)
            {
                var nameAttributes = evt.GetDomNameAttributes();
                var binding = CreateEventBinding(evt);
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
                var binding = CreateMethodBinding(method);
                
                target.AttachAll(nameAttributes, binding);
                target.AttachAll(access, binding);
            }
        }

        public static void AttachConstructors(this BindingClass target, IEnumerable<ConstructorInfo> constructors)
        {
            foreach (var constructor in constructors)
            {
                var ctorAttribute = constructor.GetDomConstructorAttribute();
                var binding = CreateConstructorBinding(constructor);
                target.AttachAll(ctorAttribute, binding);
            }
        }

        public static void AttachProperties(this BindingClass target, IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var nameAttributes = property.GetDomNameAttributes();
                var access = property.GetDomAccessorAttribute();
                var binding = CreatePropertyBinding(property);
                target.AttachAll(nameAttributes, binding);
                target.AttachAll(access, binding);
            }
        }
    }
}
