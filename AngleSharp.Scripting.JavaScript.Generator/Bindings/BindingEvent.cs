namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingEvent : BindingMember
    {
        public BindingEvent(String originalName, Type handlerType, Boolean isLenient = false)
            : base(originalName, handlerType)
        {
            IsLenient = isLenient;
        }

        public Type HandlerType
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
