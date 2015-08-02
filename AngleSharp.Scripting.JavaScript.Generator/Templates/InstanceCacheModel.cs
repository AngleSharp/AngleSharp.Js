namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;

    public class InstanceCacheModel
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

    partial class InstanceCache
    {
        public InstanceCache(InstanceCacheModel model)
        {
            Model = model;
        }

        public InstanceCacheModel Model
        {
            get;
            private set;
        }
    }
}
