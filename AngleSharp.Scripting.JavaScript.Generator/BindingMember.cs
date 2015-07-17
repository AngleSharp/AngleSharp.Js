namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    class BindingMember
    {
        public BindingMember(String name)
        {
            Name = name;
        }

        public String Name
        {
            get;
            private set;
        }
    }
}
