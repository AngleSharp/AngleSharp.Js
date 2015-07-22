namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingField : BindingMember
    {
        public BindingField(String originalName, Type valueType)
            : base(originalName)
        {
            ValueType = valueType;
        }

        public Type ValueType
        {
            get;
            private set;
        }
    }
}
