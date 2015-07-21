namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;

    public class ClassPrototypeModel
    {
        public String Namespace
        {
            get;
            set;
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

        public Boolean HasConstructor
        {
            get;
            set;
        }

        public String Prototype
        {
            get;
            set;
        }

        public IEnumerable<MethodModel> Methods
        {
            get;
            set;
        }
    }

    partial class ClassPrototype
    {
        public ClassPrototype(ClassPrototypeModel model)
        {
            Model = model;
        }

        public ClassPrototypeModel Model
        {
            get;
            private set;
        }
    }
}
