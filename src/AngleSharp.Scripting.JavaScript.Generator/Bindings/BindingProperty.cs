namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingProperty : BindingMember
    {
        public BindingProperty(String originalName, Type valueType, Boolean canRead = false, Boolean canWrite = false, Boolean isLenient = false, String forwardedTo = null)
            : base(originalName, valueType)
        {
            AllowGet = canRead;
            AllowSet = canWrite;
            IsLenient = isLenient;
            ForwardedTo = forwardedTo;
        }

        public Type ValueType
        {
            get { return Type; }
        }

        public Boolean AllowGet 
        { 
            get; 
            private set;
        }

        public Boolean AllowSet 
        {
            get;
            private set;
        }

        public Boolean IsLenient 
        {
            get;
            private set;
        }

        public String ForwardedTo
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
