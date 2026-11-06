namespace AngleSharp.Js.Cache
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Describes what building the arguments of a method needs to know about it.
    /// All of it is fixed by the method itself, so it is worked out once instead of
    /// on every call from script.
    /// </summary>
    sealed class MethodDescription
    {
        private static readonly ConcurrentDictionary<MethodBase, MethodDescription> _descriptions =
            new ConcurrentDictionary<MethodBase, MethodDescription>();

        private MethodDescription(MethodBase method)
        {
            var parameters = method.GetParameters();
            var descriptions = new ParameterDescription[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                descriptions[i] = new ParameterDescription(parameters[i]);
            }

            Parameters = descriptions;
            InitDict = method.GetCustomAttribute<DomInitDictAttribute>();
            ReturnType = method.GetCustomAttribute<DomReturnTypeAttribute>()?.ReturnType;
            TakesWindow = parameters.Length > 0 && parameters[0].ParameterType == typeof(IWindow);
            GivenConversion = (method as MethodInfo)?.ReturnType.GetGivenConversion();
            HasReceiver = TakesWindow || (method.IsStatic && method.IsDefined(typeof(ExtensionAttribute), false));
            TakesParamArray = parameters.Length > 0 &&
                parameters[parameters.Length - 1].GetCustomAttribute<ParamArrayAttribute>() != null;
        }

        /// <summary>
        /// Gets the value a script should receive for what the method returned.
        /// </summary>
        public Object ToGiven(Object value) =>
            GivenConversion == null || value == null ? value : GivenConversion.Invoke(null, new[] { value });

        public static MethodDescription Of(MethodBase method) =>
            _descriptions.GetOrAdd(method, m => new MethodDescription(m));

        public ParameterDescription[] Parameters { get; }

        public DomInitDictAttribute InitDict { get; }

        public Type ReturnType { get; }

        public Boolean TakesWindow { get; }

        /// <summary>
        /// Gets if the first parameter is not an argument of the call, i.e., it is the object
        /// an extension method extends or the window that is provided implicitly.
        /// </summary>
        public Boolean HasReceiver { get; }

        /// <summary>
        /// Gets how the value of a WebIDL union is given to a script, if the method returns one.
        /// Resolved here so that all other members only pay for looking at a null.
        /// </summary>
        public MethodInfo GivenConversion { get; }

        public Boolean TakesParamArray { get; }
    }

    /// <summary>
    /// The parts of a <see cref="ParameterInfo"/> that are read when arguments are
    /// built. Reading them from reflection means walking metadata every time.
    /// </summary>
    readonly struct ParameterDescription
    {
        public ParameterDescription(ParameterInfo parameter)
        {
            Name = parameter.Name;
            ParameterType = parameter.ParameterType;
            IsOptional = parameter.IsOptional;
            DefaultValue = parameter.IsOptional ? parameter.DefaultValue : null;
        }

        public String Name { get; }

        public Type ParameterType { get; }

        public Boolean IsOptional { get; }

        public Object DefaultValue { get; }
    }
}
