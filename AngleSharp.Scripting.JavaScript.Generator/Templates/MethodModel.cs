namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;

    public class MethodModel
    {
        public String Name
        {
            get;
            set;
        }

        public Boolean IsVoid
        {
            get;
            set;
        }

        public String OriginalName
        {
            get;
            set;
        }

        public String RefName
        {
            get;
            set;
        }

        public IEnumerable<ParameterModel> Parameters
        {
            get;
            set;
        }

        public Boolean IsLenient 
        { 
            get; 
            set; 
        }
    }
}
