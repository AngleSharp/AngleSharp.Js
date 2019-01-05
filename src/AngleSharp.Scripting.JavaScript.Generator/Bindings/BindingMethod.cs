namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingMethod : BindingFunction
    {
        public BindingMethod(String originalName, Type returnType, Boolean isLenient = false)
            : base(originalName, returnType)
        {
            IsLenient = isLenient;
        }

        public Type ReturnType 
        {
            get { return Type; }
        }

        public Boolean IsLenient
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
