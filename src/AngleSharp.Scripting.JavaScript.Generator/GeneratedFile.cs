namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.IO;
    using System.Text;

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

        /// <summary>
        /// Saves the generated file in the specified directory.
        /// </summary>
        /// <param name="directory">The directory to write to.</param>
        public void SaveIn(String directory)
        {
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, _fileName);
            File.WriteAllText(path, _content, Encoding.UTF8);
        }
    }
}
