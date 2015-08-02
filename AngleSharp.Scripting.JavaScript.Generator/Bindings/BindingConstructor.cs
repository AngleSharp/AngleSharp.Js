namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingConstructor : BindingFunction
    {
        public BindingConstructor(String originalName, Type type)
            : base(originalName, type)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
