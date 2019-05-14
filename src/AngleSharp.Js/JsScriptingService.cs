namespace AngleSharp.Scripting
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js;
    using Jint;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The JavaScript engine.
    /// </summary>
    public class JsScriptingService : IScriptingService
    {
        #region Fields

        private readonly ConditionalWeakTable<IWindow, EngineInstance> _contexts;
        private readonly Dictionary<String, Object> _external;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new JavaScript engine.
        /// </summary>
        public JsScriptingService()
        {
            _contexts = new ConditionalWeakTable<IWindow, EngineInstance>();
            _external = new Dictionary<String, Object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the external assignments.
        /// </summary>
        public IDictionary<String, Object> External => _external;

        /// <summary>
        /// Gets the engine's mime-type.
        /// </summary>
        public String Type => MimeTypeNames.DefaultJavaScript;

        #endregion

        #region Methods

        Boolean IScriptingService.SupportsType(String mimeType) =>
            MimeTypeNames.IsJavaScript(mimeType);

        /// <summary>
        /// Gets the associated Jint engine or creates it.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <returns>The engine object.</returns>
        public Engine GetOrCreateJint(IDocument document) =>
            GetOrCreateInstance(document).Jint;

        /// <summary>
        /// Evaluates the response asynchronously.
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <param name="options">The options to consider.</param>
        /// <param name="cancel">The cancellation token to transport.</param>
        public async Task EvaluateScriptAsync(IResponse response, ScriptOptions options, CancellationToken cancel)
        {
            var encoding = options.Encoding ?? Encoding.UTF8;

            using (var reader = new StreamReader(response.Content, encoding, true))
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
            document = document ?? throw new ArgumentNullException(nameof(document));
            return GetOrCreateInstance(document).RunScript(source).FromJsValue();
        }

        #endregion

        #region Helpers

        internal EngineInstance GetOrCreateInstance(IDocument document)
        {
            var objectContext = document.DefaultView;

            if (!_contexts.TryGetValue(objectContext, out var instance))
            {
                var libs = GetAssemblies(document.Context).ToArray();
                instance = new EngineInstance(objectContext, _external, libs);
                _contexts.Add(objectContext, instance);
            }

            return instance;
        }

        private static IEnumerable<Assembly> GetAssemblies(IBrowsingContext context) =>
            context.GetServices<Object>().Select(m => m.GetType().Assembly).Distinct();

        #endregion
    }
}
