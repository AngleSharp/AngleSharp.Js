namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    /// <summary>
    /// Represents the result of generating a single file.
    /// </summary>
    public class GeneratedFile
    {
        readonly BindingClass _bindingClass;
        readonly String _extension;

        internal GeneratedFile(BindingClass bindingClass, String extension)
        {
            _bindingClass = bindingClass;
            _extension = extension;
        }

        /// <summary>
        /// The name of the new file.
        /// </summary>
        public String FileName
        {
            get { return _bindingClass.Name + _extension; }
        }

        /// <summary>
        /// The content of the new file.
        /// </summary>
        public String Content
        {
            get { return String.Empty; }
        }
    }
}
