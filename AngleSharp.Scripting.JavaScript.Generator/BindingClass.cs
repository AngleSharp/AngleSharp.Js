namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingClass
    {
        readonly Dictionary<String, BindingMember> _members;

        public BindingClass(String name)
        {
            Name = name;
            _members = new Dictionary<String, BindingMember>();
        }

        public void Bind(String name, BindingMember value)
        {
            _members.Add(name, value);
        }

        public String Name
        {
            get;
            private set;
        }
    }
}
