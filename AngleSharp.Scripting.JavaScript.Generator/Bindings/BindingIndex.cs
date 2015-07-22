namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingIndex : BindingFunction
    {
        public BindingIndex(Type valueType, Boolean canRead = false, Boolean canWrite = false, Boolean isLenient = false)
            : base(String.Empty)
        {
            AllowGet = canRead;
            AllowSet = canWrite;
            IsLenient = isLenient;
            ValueType = valueType;
        }

        public Type ValueType 
        { 
            get; 
            private set;
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

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
