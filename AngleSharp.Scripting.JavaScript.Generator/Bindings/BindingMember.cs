namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    abstract class BindingMember : BindingBase
    {
        public BindingMember(String originalName, Type type)
            : base(originalName)
        {
            Type = type;
        }

        public Type Type
        {
            get;
            private set;
        }
    }
}
