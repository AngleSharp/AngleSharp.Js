namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    abstract class BindingType : BindingBase
    {
        public BindingType(String name)
        {
            Name = name;
        }

        public String Name 
        { 
            get; 
            private set; 
        }

        public abstract IEnumerable<BindingMember> GetMembers();
    }
}
