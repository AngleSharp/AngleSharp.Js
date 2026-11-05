namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Io;
    using AngleSharp.Text;
    using AngleSharp.Scripting;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents a dedicated worker as seen from a window.
    /// </summary>
    [DomName("Worker")]
    [DomExposed("Window")]
    public sealed class Worker : EventTarget
    {
        #region Fields

        private const String MessageEventName = "message";
        private const String WorkerIncomingMessageName = "__anglesharpIncomingMessage";

        private readonly IWindow _window;
        private readonly IBrowsingContext _workerContext;
        private readonly JsScriptingService _scripting;
        private readonly IEventLoop _windowLoop;
        private readonly IEventLoop _workerLoop;
        private readonly Queue<Object> _pendingMessages;

        private IDocument _workerDocument;
        private Boolean _initialized;
        private Exception _startupError;
        private Boolean _terminated;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new worker and starts the worker script in a separate context.
        /// </summary>
        public Worker(IWindow window, String source)
            : this(window, source, null)
        {
        }

        /// <summary>
        /// Creates a new worker and starts the worker script in a separate context. If that fails
        /// it is told by an error event, as it happens later.
        /// </summary>
        /// <param name="window">The window creating the worker.</param>
        /// <param name="source">The address of the script.</param>
        /// <param name="options">The options, e.g. that the script is a module.</param>
        [DomConstructor]
        public Worker(IWindow window, String source, WorkerOptions options)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            if (String.IsNullOrEmpty(source))
            {
                throw new ArgumentException("The worker source URL cannot be empty.", nameof(source));
            }

            var isModule = options?.Type == "module";

            if (!isModule && options?.Type != null && options.Type != "classic")
            {
                throw new ArgumentException($"The type '{options.Type}' is not known.", nameof(options));
            }

            _window = window;
            _windowLoop = _window.Document.Context.GetService<IEventLoop>();

            var parentContext = _window.Document.Context;
            _scripting = parentContext.GetService<JsScriptingService>() ?? throw new DomException(DomError.NotSupported);
            EnsureLoaderAvailable();
            var workerUrl = ResolveUrl(source);
            _pendingMessages = new Queue<Object>();

            // Check the parent's capabilities before starting a thread that a failed constructor
            // would leave unreachable. Release it if context creation or scheduling fails too.
            _workerLoop = new JsEventLoop();
            try
            {
                var workerConfig = Configuration.Default
                    .With(_scripting)
                    .WithOnly(_workerLoop)
                    .With<IResourceLoader>(_ => parentContext.GetService<IResourceLoader>())
                    .With<IDocumentLoader>(_ => parentContext.GetService<IDocumentLoader>());

                foreach (var service in parentContext.OriginalServices)
                {
                    if (service is EngineCreator || service is Func<IBrowsingContext, EngineCreator>)
                    {
                        workerConfig = workerConfig.With(service);
                    }
                }

                _workerContext = BrowsingContext.New(workerConfig);

                Enqueue(_workerLoop, TaskPriority.Critical, () =>
                {
                    try
                    {
                        InitializeWorker(workerUrl, isModule);
                    }
                    catch (Exception ex)
                    {
                        _startupError = ex;
                        Enqueue(_windowLoop, TaskPriority.Normal, () => ReportStartupError(workerUrl, ex));
                    }
                });
            }
            catch
            {
                try
                {
                    _workerContext?.Dispose();
                }
                finally
                {
                    ((IDisposable)_workerLoop).Dispose();
                }

                throw;
            }
        }

        #endregion

        #region Properties

        internal Exception StartupError => _startupError;

        /// <summary>
        /// Raised when the script of the worker could not be started.
        /// </summary>
        [DomName("onerror")]
        public event DomEventHandler Failed
        {
            add { AddEventListener(EventNames.Error, value); }
            remove { RemoveEventListener(EventNames.Error, value); }
        }

        internal Boolean IsInitialized => _initialized;

        internal Object EvaluateInWorker(String source)
        {
            if (_workerDocument == null)
            {
                return null;
            }

            return _scripting.EvaluateScript(_workerDocument, source, MimeTypeNames.DefaultJavaScript, _workerDocument.Url);
        }

        #endregion

        #region Events

        /// <summary>
        /// Adds or removes the handler for the message event.
        /// </summary>
        [DomName("onmessage")]
        public event DomEventHandler Message
        {
            add { AddEventListener(MessageEventName, value, false); }
            remove { RemoveEventListener(MessageEventName, value, false); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Posts a message to the running worker script.
        /// </summary>
        [DomName("postMessage")]
        public void PostMessage(Object message)
        {
            if (_terminated)
            {
                return;
            }

            //  it had its error event, and what is sent to it goes nowhere
            if (_startupError != null)
            {
                return;
            }

            if (!_initialized)
            {
                lock (_pendingMessages)
                {
                    _pendingMessages.Enqueue(message);
                }

                return;
            }

            Enqueue(_workerLoop, TaskPriority.Normal, () => DeliverToWorker(message));
        }

        /// <summary>
        /// Terminates the running worker.
        /// </summary>
        [DomName("terminate")]
        public void Terminate()
        {
            _terminated = true;

            if (_workerLoop is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        #endregion

        #region Helpers

        internal void PostMessageToOwner(Object message)
        {
            if (_terminated)
            {
                return;
            }

            var origin = _workerDocument?.Origin ?? String.Empty;
            var ev = new MessageEvent(MessageEventName, false, false, message, origin, String.Empty, null, null);
            Enqueue(_windowLoop, TaskPriority.Normal, () => Dispatch(ev));
        }

        private void ReportStartupError(Url source, Exception error)
        {
            if (!_terminated)
            {
                var ev = new ErrorEvent();
                ev.Init(source.Href, 0, 0, error);
                Dispatch(ev);
            }
        }

        private void InitializeWorker(Url sourceUrl, Boolean isModule)
        {
            _workerDocument = _workerContext.OpenAsync(request => request.Address(sourceUrl).Content("<!doctype html>")).GetAwaiter().GetResult();
            WindowExtensions.RegisterWorkerWindow(_workerDocument.DefaultView, this);
            var source = FetchWorkerScript(sourceUrl);
            _scripting.EvaluateScript(_workerDocument, WorkerBootstrap, MimeTypeNames.DefaultJavaScript, sourceUrl.Href);
            _scripting.EvaluateScript(_workerDocument, source, isModule ? "module" : MimeTypeNames.DefaultJavaScript, sourceUrl.Href);

            _initialized = true;

            lock (_pendingMessages)
            {
                while (_pendingMessages.Count > 0)
                {
                    DeliverToWorker(_pendingMessages.Dequeue());
                }
            }
        }

        private void DeliverToWorker(Object message)
        {
            var engine = _scripting.GetOrCreateJint(_workerDocument);

            //  An array would reach the other engine as a wrapper of the CLR object.
            if (message is Byte[] bytes)
            {
                engine.SetValue(WorkerIncomingMessageName, engine.Intrinsics.ArrayBuffer.Construct(bytes));
            }
            else
            {
                engine.SetValue(WorkerIncomingMessageName, message);
            }

            _scripting.EvaluateScript(_workerDocument, WorkerDispatchScript, MimeTypeNames.DefaultJavaScript, _workerDocument.Url);
        }

        private String FetchWorkerScript(Url source)
        {
            var loader = _window.Document.Context.GetService<IDocumentLoader>();

            if (loader == null)
            {
                throw new DomException(DomError.NotSupported);
            }

            var request = new DocumentRequest(source)
            {
                Referer = _window.Document.DocumentUri,
            };

            using (var response = loader.FetchAsync(request).Task.GetAwaiter().GetResult())
            {
                if (response?.Content == null)
                {
                    throw new DomException(DomError.NotSupported);
                }

                using (var reader = new StreamReader(response.Content))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private void EnsureLoaderAvailable()
        {
            var loader = _window.Document.Context.GetService<IDocumentLoader>();

            if (loader == null)
            {
                throw new DomException(DomError.NotSupported);
            }
        }

        private Url ResolveUrl(String source)
        {
            if (source.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                source.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                source.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return Url.Create(source);
            }

            var documentUrl = _window.Document.Url;

            if (String.IsNullOrEmpty(documentUrl) || documentUrl.Is("about:blank"))
            {
                documentUrl = "https://example.com/";
            }

            return new Url(new Url(documentUrl), source);
        }

        private static void Enqueue(IEventLoop loop, TaskPriority priority, Action action)
        {
            if (loop != null)
            {
                loop.Enqueue(() => action(), priority);
            }
            else
            {
                action();
            }
        }

        private const String WorkerBootstrap = @"(function () {
if (typeof self === 'undefined') {
    this.self = this;
}
})();";

        private const String WorkerDispatchScript = @"(function () {
if (self && typeof self.onmessage === 'function') {
    self.onmessage({ data: __anglesharpIncomingMessage });
}
    })();";

        #endregion

    }

    /// <summary>
    /// Represents the options of creating a worker.
    /// </summary>
    [DomName("WorkerOptions")]
    public sealed class WorkerOptions
    {
        /// <summary>
        /// Gets or sets the type of the script, i.e. classic (the default) or module.
        /// </summary>
        [DomName("type")]
        public String Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the worker, for debugging.
        /// </summary>
        [DomName("name")]
        public String Name { get; set; }
    }
}
