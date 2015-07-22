namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class BindingExtensions
    {
        static BindingIndex CreateIndexBinding(PropertyInfo propertyInfo)
        {
            var lenient = propertyInfo.GetDomLenientThisAttribute();
            return new BindingIndex(
                canRead: propertyInfo.CanRead,
                canWrite: propertyInfo.CanWrite,
                isLenient: lenient != null,
                valueType: propertyInfo.PropertyType);
        }

        static BindingProperty CreatePropertyBinding(PropertyInfo propertyInfo)
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

        static BindingMember CreatePropertyOrIndexBinding(PropertyInfo propertyInfo)
        {
            var parameters = propertyInfo.GetIndexParameters();

            if (parameters.Length > 0)
                return CreateIndexBinding(propertyInfo).With(parameters);

            return CreatePropertyBinding(propertyInfo);
        }

        static BindingConstructor CreateConstructorBinding(ConstructorInfo constructorInfo)
        {
            return new BindingConstructor(
                originalName: constructorInfo.DeclaringType.Name).With(
                constructorInfo.GetParameters());
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
                returnType: methodInfo.ReturnType).With(
                methodInfo.GetParameters());
        }

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

        public static void AttachEvents(this BindingClass target, IEnumerable<EventInfo> events)
        {
            foreach (var evt in events)
            {
                var nameAttributes = evt.GetDomNameAttributes();
                var binding = CreateEventBinding(evt);

                target.AttachAll(nameAttributes, binding);
            }
        }

        static T With<T>(this T function, ParameterInfo[] parameters)
            where T : BindingFunction
        {
            foreach (var parameter in parameters)
            {
                function.Bind(new BindingParameter(
                    originalName: parameter.Name,
                    valueType: parameter.ParameterType,
                    position: parameter.Position,
                    optional: parameter.IsOptional
                ));
            }

            return function;
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
                var binding = CreatePropertyOrIndexBinding(property);

                target.AttachAll(nameAttributes, binding);
                target.AttachAll(access, binding);
            }
        }

        public static IEnumerable<Type> ResolveTypes(this IEnumerable<BindingType> bindings)
        {
            var visitor = new TypeVisitor();

            foreach (var binding in bindings)
                binding.Accept(visitor);

            return visitor.Dependencies;
        }
    }
}
