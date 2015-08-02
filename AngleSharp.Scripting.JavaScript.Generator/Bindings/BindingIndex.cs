namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingIndex : BindingFunction
    {
        public BindingIndex(Type valueType, Boolean canRead = false, Boolean canWrite = false, Boolean isLenient = false)
            : base(String.Empty, valueType)
        {
            AllowGet = canRead;
            AllowSet = canWrite;
            IsLenient = isLenient;
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

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
