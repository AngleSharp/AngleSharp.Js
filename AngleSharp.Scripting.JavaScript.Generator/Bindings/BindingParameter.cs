namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingParameter : BindingMember
    {
        public BindingParameter(String originalName, Type valueType, Int32 position, Boolean optional = false)
            : base(originalName)
        {
            ValueType = valueType;
            Position = position;
            IsOptional = optional;
        }

        public Type ValueType
        {
            get;
            private set;
        }

        public Boolean IsOptional
        {
            get;
            private set;
        }

        public Int32 Position
        {
            get;
            private set;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
