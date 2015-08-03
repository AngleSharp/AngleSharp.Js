namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;

    public class ParameterModel
    {
        public String Name
        {
            get;
            set;
        }

        public Int32 Index
        {
            get;
            set;
        }

        public String Converter
        {
            get;
            set;
        }

        public Boolean IsOptional
        {
            get;
            set;
        }

        public Type ParameterType
        {
            get;
            set;
        }
    }
}
