namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    abstract class BindingType : BindingBase
    {
        public BindingType(String name, String originalName, String originalNamespace)
            : base(originalName)
        {
            Name = name;
            OriginalNamespace = originalNamespace;
        }

        public String Name 
        { 
            get; 
            private set; 
        }

        public String OriginalNamespace
        {
            get;
            private set;
        }

        public abstract IEnumerable<BindingMember> GetMembers();
    }
}
