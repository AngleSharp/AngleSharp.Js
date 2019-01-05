namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingField : BindingMember
    {
        public BindingField(String originalName, Type valueType)
            : base(originalName, valueType)
        {
        }

        public Type ValueType
        {
            get { return Type; }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
