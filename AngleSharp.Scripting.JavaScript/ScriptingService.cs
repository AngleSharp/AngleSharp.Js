namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Network;
    using AngleSharp.Services;
    using System;

    class ScriptingService : IScriptingService
    {
        public IScriptEngine GetEngine(String mimeType)
        {
            if (MimeTypes.IsJavaScript(mimeType))
                return new JavaScriptEngine();

            return null;
        }
    }
}
