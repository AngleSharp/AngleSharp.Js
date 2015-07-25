namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class BindingExtensions
    {
        public static BindingIndex CreateIndexBinding(this PropertyInfo propertyInfo)
        {
            var lenient = propertyInfo.GetDomLenientThisAttribute();
            return new BindingIndex(
                canRead: propertyInfo.CanRead,
                canWrite: propertyInfo.CanWrite,
                isLenient: lenient != null,
                valueType: propertyInfo.PropertyType);
        }

        public static BindingProperty CreatePropertyBinding(this PropertyInfo propertyInfo)
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

        public static BindingMember CreatePropertyOrIndexBinding(this PropertyInfo propertyInfo)
        {
            var parameters = propertyInfo.GetIndexParameters();

            if (parameters.Length > 0)
                return CreateIndexBinding(propertyInfo).With(parameters);

            return CreatePropertyBinding(propertyInfo);
        }

        public static BindingConstructor CreateConstructorBinding(this ConstructorInfo constructorInfo)
        {
            return new BindingConstructor(
                originalName: constructorInfo.DeclaringType.Name).With(
                constructorInfo.GetParameters());
        }

        public static BindingEvent CreateEventBinding(this EventInfo eventInfo)
        {
            var lenient = eventInfo.GetDomLenientThisAttribute();
            return new BindingEvent(
                originalName: eventInfo.Name,
                handlerType: eventInfo.EventHandlerType,
                isLenient: lenient != null);
        }

        public static BindingMethod CreateMethodBinding(this MethodInfo methodInfo)
        {
            return new BindingMethod(
                originalName: methodInfo.Name,
                returnType: methodInfo.ReturnType).With(
                methodInfo.GetParameters());
        }

        public static IEnumerable<Type> ResolveTypes(this IEnumerable<BindingType> bindings)
        {
            var visitor = new TypeVisitor();

            foreach (var binding in bindings)
                binding.Accept(visitor);

            return visitor.Dependencies;
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
    }
}
