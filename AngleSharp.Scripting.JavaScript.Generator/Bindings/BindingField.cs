namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

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

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
