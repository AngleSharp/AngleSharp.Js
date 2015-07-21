namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;

    /// <summary>
    /// Represents the result of generating a single file.
    /// </summary>
    public class GeneratedFile
    {
        readonly String _content;
        readonly String _fileName;

        internal GeneratedFile(String content, String fileName)
        {
            _content = content;
            _fileName = fileName;
        }

        /// <summary>
        /// The name of the new file.
        /// </summary>
        public String FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// The content of the new file.
        /// </summary>
        public String Content
        {
            get { return _content; }
        }
    }
}
