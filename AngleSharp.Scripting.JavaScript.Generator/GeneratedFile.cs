namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    /// <summary>
    /// Represents the result of generating a single file.
    /// </summary>
    public class GeneratedFile
    {
        readonly BindingClass _bindingClass;
        readonly Options _options;

        internal GeneratedFile(BindingClass bindingClass, Options options)
        {
            _bindingClass = bindingClass;
            _options = options;
        }

        /// <summary>
        /// The name of the new file.
        /// </summary>
        public String FileName
        {
            get { return _bindingClass.Name + _options.Extension; }
        }

        /// <summary>
        /// The content of the new file.
        /// </summary>
        public String Content
        {
            get 
            {
                var writer = new SyntaxWriter(_options.Namespace);
                writer.AddClass(_bindingClass);
                return writer.Serialize(); 
            }
        }
    }
}
