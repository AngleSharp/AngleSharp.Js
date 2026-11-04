namespace AngleSharp.Scripting
{
    using Acornima;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js;
    using AngleSharp.Js.Dom;
    using AngleSharp.Text;
    using Jint;
    using Jint.Runtime;
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
        private readonly JsScriptingOptions _options;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new JavaScript engine using the default options.
        /// </summary>
        public JsScriptingService()
            : this(new JsScriptingOptions())
        {
        }

        /// <summary>
        /// Creates a new JavaScript engine using the given options. The options
        /// are copied, so that editing them afterwards leaves this engine alone.
        /// </summary>
        /// <param name="options">The options tuning the engine.</param>
        public JsScriptingService(JsScriptingOptions options)
        {
            _contexts = new ConditionalWeakTable<IWindow, EngineInstance>();
            _external = new Dictionary<String, Object>();
            _options = (options ?? throw new ArgumentNullException(nameof(options))).Clone();
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
            MimeTypeNames.IsJavaScript(mimeType) ||
            mimeType.Isi("module") ||
            mimeType.Isi("importmap");

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
                var type = options.Element?.Type;
                //  A module is imported by this, so it has to tell it apart from other modules:
                //  one written into the document would have the address of the document.
                var isModule = type.Isi("module");
                var sourceUrl = isModule ? options.Element?.Source : response.Address?.Href ?? options.Element?.Source;
                var position = isModule || !String.IsNullOrEmpty(options.Element?.Source) ? default : FindInDocument(options.Document, content);
                await options.EventLoop.EnqueueAsync(_ =>
                {
                    var document = options.Document ?? throw new ArgumentNullException(nameof(options));

                    // a sandboxed frame shows its document, and runs nothing of it
                    if (OriginPolicy.IsScriptingBlocked(document))
                    {
                        return null;
                    }

                    var engine = GetOrCreateInstance(document);

                    try
                    {
                        return engine.RunScript(content, type, sourceUrl, position).FromJsValue();
                    }
                    catch (JavaScriptException error)
                    {
                        //  the script is over, and nothing but it was waiting for it
                        engine.ReportError(error);
                        return null;
                    }
                }, TaskPriority.Critical).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Finds a script written into the document, which is being parsed up to the end of it. The
        /// lines of a script are normalized by the parser, while the source of the document isn't.
        /// The text has to be all of an element to count: a script not found that way was not written
        /// there (e.g. added by another script, even with its text being in a string of that one).
        /// </summary>
        private static Position FindInDocument(IDocument document, String content)
        {
            var source = document?.Source;

            if (source == null || content.Length == 0)
            {
                return default;
            }

            var text = source.Text;
            text = text.Substring(0, Math.Min(source.Index, text.Length));

            if (text.IndexOf('\r') >= 0)
            {
                text = text.Replace("\r\n", "\n").Replace('\r', '\n');
            }

            var index = text.LastIndexOf(content, StringComparison.Ordinal);

            while (index > 0 && !IsScriptText(text, index, content.Length))
            {
                index = text.LastIndexOf(content, index - 1, StringComparison.Ordinal);
            }

            if (index <= 0)
            {
                return default;
            }

            var line = 1;
            var lineStart = 0;

            for (var i = text.IndexOf('\n', 0, index); i >= 0; i = text.IndexOf('\n', i + 1, index - i - 1))
            {
                line++;
                lineStart = i + 1;
            }

            return Position.From(line, index - lineStart);
        }

        private static Boolean IsScriptText(String text, Int32 index, Int32 length) =>
            text[index - 1] == '>' && String.Compare(text, index + length, "</script", 0, 8, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Evaluates the given script source in the engine of the document.
        /// </summary>
        /// <param name="document">The context of the evaluation.</param>
        /// <param name="source">The source of the script.</param>
        /// <param name="type">The type of the script.</param>
        /// <param name="sourceUrl">The URL of the script.</param>
        /// <returns>The result of the evaluation.</returns>
        public Object EvaluateScript(IDocument document, String source, String type, String sourceUrl)
        {
            document = document ?? throw new ArgumentNullException(nameof(document));

            // a sandboxed frame shows its document, and runs nothing of it
            if (OriginPolicy.IsScriptingBlocked(document))
            {
                return null;
            }

            return GetOrCreateInstance(document).RunScript(source, type, sourceUrl).FromJsValue();
        }

        #endregion

        #region Helpers

        internal EngineInstance GetOrCreateInstance(IDocument document)
        {
            var objectContext = document.DefaultView;

            if (!_contexts.TryGetValue(objectContext, out var instance))
            {
                var libs = GetAssemblies(document.Context).ToArray();
                instance = new EngineInstance(objectContext, _external, libs, _options);
                _contexts.Add(objectContext, instance);
            }

            return instance;
        }

        private static IEnumerable<Assembly> GetAssemblies(IBrowsingContext context) => context
            .GetServices<Object>()
            .Select(m => m.GetType().GetAssembly())
            .Distinct()
            .Where(m => m.FullName.StartsWith("AngleSharp"));

        #endregion
    }
}
