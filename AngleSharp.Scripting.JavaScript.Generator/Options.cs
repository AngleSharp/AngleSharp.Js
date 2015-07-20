namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a set of options for the transformation process.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Creates the default options.
        /// </summary>
        public Options()
        {
            Extension = ".g.cs";
            Namespace = "AngleSharp";
            TypeMapping = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Gets or sets the extension to use. By default this is ".g.cs".
        /// </summary>
        public String Extension
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the namespace to use. By default this is "AngleSharp".
        /// </summary>
        public String Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the defined type mapping. By default nothing is mapped.
        /// </summary>
        public Dictionary<Type, Type> TypeMapping
        {
            get;
            private set;
        }
    }
}
