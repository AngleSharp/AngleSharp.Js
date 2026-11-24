namespace AngleSharp.Scripting
{
    using Acornima;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js;
    using AngleSharp.Js.Cache;
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
        private readonly Lazy<JsDomSurface> _surface;

        #endregion

        #region Test Support

        /// <summary>
        /// Gets or sets the registration a service given none uses instead of reflection. Only the
        /// test suite sets it, to run every test against the registered DOM as well.
        /// </summary>
        internal static DomRegistration DefaultDomRegistration { get; set; }

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

            //  Built on the first document that runs a script, once for every one after it: what a
            //  registration declares does not depend on the document, and it may declare a lot.
            //  The DOM of AngleSharp itself and of this library always come first: this library's
            //  builds on the core one (MessagePort is an EventTarget), and as under reflection a
            //  library implementing one of its stubs (the renderer's layout members) wins over it.
            //  Only the libraries beyond these two are for the registration to name.
            var registration = _options.DomRegistration ?? DefaultDomRegistration;
            _surface = registration != null
                ? new Lazy<JsDomSurface>(() => JsDomSurface.Create(DomRegistrations.Combine(CoreDom.Register, JsDom.Register, registration)), LazyThreadSafetyMode.ExecutionAndPublication)
                : null;
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
            //  a script inserted by script ran then already (see InsertedScripts)
            if (InsertedScripts.TakeRan(options.Element))
            {
                return;
            }

            var encoding = options.Encoding ?? Encoding.UTF8;

            using (var reader = new StreamReader(response.Content, encoding, true))
            {
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);
                var type = options.Element?.Type;
                //  A module is imported by this, so it has to tell it apart from other modules:
                //  one written into the document would have the address of the document. One that
                //  was loaded has its address, which is what the modules it imports resolve against.
                var isModule = type.Isi("module");
                var isInline = String.IsNullOrEmpty(options.Element?.Source);
                var sourceUrl = isModule && isInline ? null : response.Address?.Href ?? options.Element?.Source;
                var position = isInline ? FindInDocument(options.Document, content) : default;
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

        /// <summary>
        /// Gets the engine of the given window, if its document ran scripts already.
        /// </summary>
        internal static EngineInstance TryGetInstance(IWindow window) =>
            window?.Document?.Context?.GetService<JsScriptingService>() is { } service && service._contexts.TryGetValue(window, out var instance)
                ? instance
                : null;

        internal EngineInstance GetOrCreateInstance(IDocument document)
        {
            var objectContext = document.DefaultView;

            if (!_contexts.TryGetValue(objectContext, out var instance))
            {
                var surface = _surface?.Value;
                var libs = surface != null ? Array.Empty<Assembly>() : GetAssemblies(document.Context).ToArray();
                instance = new EngineInstance(objectContext, _external, libs, _options, surface);
                _contexts.Add(objectContext, instance);
            }

            return instance;
        }

        private static IEnumerable<Assembly> GetAssemblies(IBrowsingContext context) => context
            .GetServices<Object>()
            .SelectMany(GetAssemblies)
            .Distinct()
            .Where(m => m.FullName.StartsWith("AngleSharp"));

        //  A service registered by a creator (e.g. WithWasm's runtime factory) is only created when
        //  first asked for, so until then it is the delegate, a type of the framework: the library
        //  is the one of the type it creates and of its code.
        private static IEnumerable<Assembly> GetAssemblies(Object service)
        {
            yield return service.GetType().GetAssembly();

            if (service is Delegate creator)
            {
                var type = creator.GetType();

                if (type.GetTypeInfo().IsGenericType)
                {
                    foreach (var argument in type.GetTypeInfo().GenericTypeArguments)
                    {
                        yield return argument.GetAssembly();
                    }
                }

                if (creator.GetMethodInfo()?.DeclaringType is Type declaring)
                {
                    yield return declaring.GetAssembly();
                }
            }
        }

        #endregion
    }
}
