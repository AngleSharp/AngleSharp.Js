namespace AngleSharp.Scripting.JavaScript.Services
{
    using AngleSharp.Network;
    using AngleSharp.Services;
    using AngleSharp.Services.Scripting;
    using System;

    /// <summary>
    /// Represents the service for the JavaScript engine.
    /// </summary>
    public class ScriptingService : IScriptingService
    {
        readonly JavaScriptEngine _engine;

        /// <summary>
        /// Creates a new scripting service.
        /// </summary>
        public ScriptingService()
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
            if (MimeTypes.IsJavaScript(mimeType))
                return _engine;

            return null;
        }
    }
}
