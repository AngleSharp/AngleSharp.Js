namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

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
        }

        /// <summary>
        /// Gets or sets the extension to use. By default this is ".g.cs".
        /// </summary>
        public String Extension
        {
            get;
            set;
        }
    }
}
