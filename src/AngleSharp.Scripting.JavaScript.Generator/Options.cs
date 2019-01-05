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
            Namespace = "AngleSharp.Scripting.JavaScript";
            TypeConverters = new Dictionary<Type, String>();
        }

        /// <summary>
        /// Creates a new set of options for creating Jint bindings.
        /// </summary>
        /// <returns>The ready-to-use options.</returns>
        public static Options ForJint()
        {
            return new Options().UseDomConverters().
                                 UseJintConverters().
                                 UseSystemConverters();
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
        /// Gets or sets the namespace to use. By default this is 
        /// "AngleSharp.Scripting.JavaScript".
        /// </summary>
        public String Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the defined type converters. By default nothing is defined.
        /// </summary>
        public Dictionary<Type, String> TypeConverters
        {
            get;
            private set;
        }
    }
}
