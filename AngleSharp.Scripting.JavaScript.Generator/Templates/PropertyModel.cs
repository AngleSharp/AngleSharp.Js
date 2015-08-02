namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;

    public class PropertyModel
    {
        public String Name
        {
            get;
            set;
        }

        public MethodModel Getter
        {
            get;
            set;
        }

        public MethodModel Setter
        {
            get;
            set;
        }
    }
}
