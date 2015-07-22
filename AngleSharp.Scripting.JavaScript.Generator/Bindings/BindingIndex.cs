namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingIndex : BindingMember
    {
        public BindingIndex(Type valueType, Boolean canRead = false, Boolean canWrite = false, Boolean isLenient = false, Dictionary<String, Type> parameters = null)
            : base(String.Empty)
        {
            AllowGet = canRead;
            AllowSet = canWrite;
            IsLenient = isLenient;
            ValueType = valueType;
            Parameters = parameters ?? new Dictionary<String, Type>();
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

        public Dictionary<String, Type> Parameters
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
