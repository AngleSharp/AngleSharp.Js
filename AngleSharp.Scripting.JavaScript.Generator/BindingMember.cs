namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    abstract class BindingMember
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
