namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;

    public class ClassInstanceModel
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

        public String BaseName
        {
            get;
            set;
        }

        public String OriginalName
        {
            get;
            set;
        }

        public IEnumerable<FieldModel> Fields
        {
            get;
            set;
        }
    }

    partial class ClassInstance
    {
        public ClassInstance(ClassInstanceModel model)
        {
            Model = model;
        }

        public ClassInstanceModel Model
        {
            get;
            private set;
        }
    }
}
