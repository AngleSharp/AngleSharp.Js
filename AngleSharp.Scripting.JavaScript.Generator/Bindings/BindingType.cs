namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    abstract class BindingType : BindingBase
    {
        public BindingType(String name)
        {
            Name = name;
        }

        public String Name 
        { 
            get; 
            private set; 
        }

        public abstract void Serialize(SyntaxWriter writer);
    }
}
