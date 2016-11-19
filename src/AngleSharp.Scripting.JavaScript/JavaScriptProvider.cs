namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Io;
    using System;

    /// <summary>
    /// Represents the service for the JavaScript engine.
    /// </summary>
    public class JavaScriptProvider : IScriptingProvider
    {
        private readonly JavaScriptEngine _engine;

        /// <summary>
        /// Creates a new scripting service.
        /// </summary>
        public JavaScriptProvider()
        {
            _engine = new JavaScriptEngine();
        }

        /// <summary>
        /// Gets the available JavaScript engine.
        /// </summary>
        public JavaScriptEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Gets the registered engine for the provided mime-type.
        /// </summary>
        /// <param name="mimeType">The mime-type.</param>
        /// <returns>The contained engine.</returns>
        public virtual IScriptEngine GetEngine(String mimeType)
        {
            if (MimeTypeNames.IsJavaScript(mimeType))
            {
                return _engine;
            }

            return null;
        }
    }
}
