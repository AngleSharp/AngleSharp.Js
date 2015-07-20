namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    sealed class BindingProperty : BindingMember
    {
        public BindingProperty(String originalName, Type valueType, Boolean canRead = false, Boolean canWrite = false, Boolean isLenient = false, String forwardedTo = null)
            : base(originalName)
        {
            AllowGet = canRead;
            AllowSet = canWrite;
            IsLenient = isLenient;
            ForwardedTo = forwardedTo;
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

        public String ForwardedTo
        {
            get;
            private set;
        }
    }
}
