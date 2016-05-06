namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    abstract class BindingBase
    {
        public BindingBase(String originalName)
        {
            OriginalName = originalName;
        }

        public String OriginalName
        {
            get;
            private set;
        }

        public abstract void Accept(IVisitor visitor);
    }
}
