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
    using System.Threading;
    using System.Threading.Tasks;

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
        /// Evaluates the response and returns an already finished task.
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <param name="options">The options to consider.</param>
        /// <param name="cancel">The currently unused cancellation token.</param>
        /// <returns>The completed task.</returns>
        public Task EvaluateScriptAsync(IResponse response, ScriptOptions options, CancellationToken cancel)
        {
            var reader = new StreamReader(response.Content, options.Encoding ?? Encoding.UTF8, true);
            var content = reader.ReadToEnd();
            reader.Close();
            var objectContext = options.Context;
            var instance = default(EngineInstance);

            if (!_contexts.TryGetValue(objectContext, out instance))
            {
                _contexts.Add(objectContext, instance = new EngineInstance(objectContext, _external));
            }

            instance.RunScript(content);
            return Task.FromResult(true);
        }
    }
}
