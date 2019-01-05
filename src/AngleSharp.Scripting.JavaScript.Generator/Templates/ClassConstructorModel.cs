namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ClassConstructorModel
    {
        public String Namespace
        {
            get;
            set;
        }

        public Int32 ConstructorLength
        {
            get
            {
                if (Constructors.Any() == false)
                    return 0;

                return Constructors.Select(m => m.Parameters.Count()).Max();
            }
        }

        public String OriginalNamespace
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String OriginalName
        {
            get;
            set;
        }

        public IEnumerable<MethodModel> Constructors 
        { 
            get; 
            set; 
        }

        public IEnumerable<FieldModel> Constants
        {
            get;
            set;
        }
    }

    partial class ClassConstructor
    {
        public ClassConstructor(ClassConstructorModel model)
        {
            Model = model;
        }

        public ClassConstructorModel Model
        {
            get;
            private set;
        }
    }
}
