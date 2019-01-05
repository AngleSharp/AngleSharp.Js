namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;

    public class DomConstructorsModel
    {
        public String Namespace
        {
            get;
            set;
        }

        public IEnumerable<String> Constructors
        {
            get;
            set;
        }
    }

    partial class DomConstructors
    {
        public DomConstructors(DomConstructorsModel model)
        {
            Model = model;
        }

        public DomConstructorsModel Model
        {
            get;
            private set;
        }
    }
}
