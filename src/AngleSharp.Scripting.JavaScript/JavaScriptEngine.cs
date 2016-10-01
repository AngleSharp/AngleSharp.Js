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
        #region Fields

        private readonly ConditionalWeakTable<IWindow, EngineInstance> _contexts;
        private readonly Dictionary<String, Object> _external;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new JavaScript engine.
        /// </summary>
        public JavaScriptEngine()
        {
            _contexts = new ConditionalWeakTable<IWindow, EngineInstance>();
            _external = new Dictionary<String, Object>();
        }

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        /// <summary>
        /// Gets the associated Jint engine or creates it.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <returns>The engine object.</returns>
        public Engine GetOrCreateJint(IDocument document)
        {
            return GetOrCreateInstance(document).Jint;
        }

        /// <summary>
        /// Evaluates the response asynchronously.
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <param name="options">The options to consider.</param>
        /// <param name="cancel">The cancellation token to transport.</param>
        public async Task EvaluateScriptAsync(IResponse response, ScriptOptions options, CancellationToken cancel)
        {
            using (var reader = new StreamReader(response.Content, options.Encoding ?? Encoding.UTF8, true))
            {
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);
                EvaluateScript(options.Document, content);
            }
        }

        /// <summary>
        /// Evaluates the given script source in the engine of the document.
        /// </summary>
        /// <param name="document">The context of the evaluation.</param>
        /// <param name="source">The source of the script.</param>
        /// <returns>The result of the evaluation.</returns>
        public Object EvaluateScript(IDocument document, String source)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            
            return GetOrCreateInstance(document).RunScript(source).FromJsValue();
        }

        #endregion

        #region Helpers

        private EngineInstance GetOrCreateInstance(IDocument document)
        {
            var instance = default(EngineInstance);
            var objectContext = document.DefaultView;

            if (!_contexts.TryGetValue(objectContext, out instance))
            {
                instance = new EngineInstance(objectContext, _external);
                _contexts.Add(objectContext, instance);
            }

            return instance;
        }

        #endregion
    }
}
