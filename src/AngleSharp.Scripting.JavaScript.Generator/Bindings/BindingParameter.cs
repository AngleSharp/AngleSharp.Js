namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingParameter : BindingMember
    {
        public BindingParameter(String originalName, Type valueType, Int32 position, Boolean optional = false, Boolean parameters = false)
            : base(originalName, valueType)
        {
            Position = position;
            IsOptional = optional;
            IsParams = parameters;
        }

        public Type ValueType
        {
            get { return Type; }
        }

        public Boolean IsOptional
        {
            get;
            private set;
        }

        public Boolean IsParams
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
