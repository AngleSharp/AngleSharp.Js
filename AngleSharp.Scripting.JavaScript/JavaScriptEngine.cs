namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using AngleSharp.Services.Scripting;
    using Jint;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// The JavaScript engine.
    /// </summary>
    public class JavaScriptEngine : IScriptEngine
    {
        readonly ConditionalWeakTable<IWindow, EngineInstance> _contexts;
        readonly Dictionary<String, Object> _external;

        /// <summary>
        /// Creates a new JavaScript engine.
        /// </summary>
        public JavaScriptEngine()
        {
            _contexts = new ConditionalWeakTable<IWindow, EngineInstance>();
            _external = new Dictionary<String, Object>();
        }

        /// <summary>
        /// Gets the external assignments.
        /// </summary>
        public IDictionary<String, Object> External
        {
            get { return _external; }
        }

        /// <summary>
        /// Gets the engine's mime-type.
        /// </summary>
        public String Type
        {
            get { return MimeTypeNames.DefaultJavaScript; }
        }

        /// <summary>
        /// Gets the associated Jint engine, if any.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <returns>The engine object, if any.</returns>
        public Engine GetJint(IDocument document)
        {
            var instance = default(EngineInstance);

            if (_contexts.TryGetValue(document.DefaultView, out instance))
            {
                return instance.Jint;
            }

            return null;
        }

        /// <summary>
        /// Evaluates the given source.
        /// </summary>
        /// <param name="source">The source code to evaluate.</param>
        /// <param name="options">The options to consider.</param>
        public void Evaluate(String source, ScriptOptions options)
        {
            var objectContext = options.Context;
            var instance = default(EngineInstance);

            if (!_contexts.TryGetValue(objectContext, out instance))
            {
                _contexts.Add(objectContext, instance = new EngineInstance(objectContext, _external));
            }

            instance.RunScript(source);
        }

        /// <summary>
        /// Evaluates the response.
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <param name="options">The options to consider.</param>
        public void Evaluate(IResponse response, ScriptOptions options)
        {
            var reader = new StreamReader(response.Content, options.Encoding ?? Encoding.UTF8, true);
            var content = reader.ReadToEnd();
            reader.Close();
            Evaluate(content, options);
        }
    }
}
