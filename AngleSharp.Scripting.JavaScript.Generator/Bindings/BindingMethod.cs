namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingMethod : BindingMember
    {
        public BindingMethod(String originalName, Type returnType, Dictionary<String, Type> parameters = null)
            : base(originalName)
        {
            Parameters = parameters ?? new Dictionary<String, Type>();
            ReturnType = returnType;
        }

        public Dictionary<String, Type> Parameters 
        { 
            get; 
            private set; 
        }

        public Type ReturnType 
        { 
            get; 
            private set; 
        }
    }
}
