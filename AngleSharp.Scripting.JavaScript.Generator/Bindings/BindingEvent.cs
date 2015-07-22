namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingEvent : BindingMember
    {
        public BindingEvent(String originalName, Type handlerType, Boolean isLenient = false)
            : base(originalName)
        {
            HandlerType = handlerType;
            IsLenient = isLenient;
        }

        public Type HandlerType 
        { 
            get; 
            private set; 
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
