namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingConstructor : BindingMember
    {
        public BindingConstructor(String originalName, Dictionary<String, Type> parameters = null)
            : base(originalName)
        {
            Parameters = parameters ?? new Dictionary<String, Type>();
        }

        public Dictionary<String, Type> Parameters 
        { 
            get; 
            private set; 
        }
    }
}
