namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    abstract class BindingMember : BindingBase
    {
        public BindingMember(String originalName)
        {
            OriginalName = originalName;
        }

        public String OriginalName
        {
            get;
            private set;
        }
    }
}
