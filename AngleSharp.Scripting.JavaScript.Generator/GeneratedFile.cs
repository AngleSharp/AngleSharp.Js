namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    /// <summary>
    /// Represents the result of generating a single file.
    /// </summary>
    public class GeneratedFile
    {
        readonly BindingType _type;
        readonly Options _options;

        internal GeneratedFile(BindingType type, Options options)
        {
            _type = type;
            _options = options;
        }

        /// <summary>
        /// The name of the new file.
        /// </summary>
        public String FileName
        {
            get { return _type.Name + _options.Extension; }
        }

        /// <summary>
        /// The content of the new file.
        /// </summary>
        public String Content
        {
            get 
            {
                var writer = new SyntaxWriter(_options.Namespace);
                _type.Serialize(writer);
                return writer.Serialize(); 
            }
        }
    }
}
