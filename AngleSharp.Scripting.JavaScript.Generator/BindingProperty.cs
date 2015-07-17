namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    class BindingProperty : BindingMember
    {
        public BindingProperty(String name)
            : base(name)
        {
        }

        public String OriginalName 
        { 
            get; 
            set; 
        }

        public Boolean AllowGet 
        { 
            get; 
            set;
        }

        public Boolean AllowSet 
        { 
            get; 
            set; 
        }

        public Boolean IsLenient 
        { 
            get; 
            set;
        }

        public String ForwardedTo
        { 
            get; 
            set; 
        }
    }
}
