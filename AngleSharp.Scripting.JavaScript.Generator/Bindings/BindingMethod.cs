namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingMethod : BindingFunction
    {
        public BindingMethod(String originalName, Type returnType)
            : base(originalName)
        {
            ReturnType = returnType;
        }

        public Type ReturnType 
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
