namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class BindingExtensions
    {
        public static BindingIndex CreateIndexBinding(this PropertyInfo propertyInfo)
        {
            return new BindingIndex(
                canRead: propertyInfo.CanRead,
                canWrite: propertyInfo.CanWrite,
                isLenient: propertyInfo.HasLenientThis(),
                valueType: propertyInfo.PropertyType);
        }

        public static BindingProperty CreatePropertyBinding(this PropertyInfo propertyInfo)
        {
            return new BindingProperty(
                originalName: propertyInfo.Name,
                canRead: propertyInfo.CanRead && propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsPublic,
                canWrite: propertyInfo.CanWrite && propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsPublic,
                isLenient: propertyInfo.HasLenientThis(),
                forwardedTo: propertyInfo.GetPutForwardsTo(),
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
                originalName: constructorInfo.DeclaringType.Name,
                type: constructorInfo.DeclaringType).With(
                constructorInfo.GetParameters());
        }

        public static BindingEvent CreateEventBinding(this EventInfo eventInfo)
        {
            return new BindingEvent(
                originalName: eventInfo.Name,
                handlerType: eventInfo.EventHandlerType,
                isLenient: eventInfo.HasLenientThis());
        }

        public static BindingMethod CreateMethodBinding(this MethodInfo methodInfo)
        {
            return new BindingMethod(
                originalName: methodInfo.Name,
                isLenient: methodInfo.HasLenientThis(),
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
                var isparams = parameter.IsParams();
                var type = isparams ? parameter.ParameterType.GetElementType() : parameter.ParameterType;

                function.Bind(new BindingParameter(
                    originalName: parameter.Name,
                    valueType: type,
                    position: parameter.Position,
                    optional: parameter.IsOptional,
                    parameters: isparams
                ));
            }

            return function;
        }
    }
}
