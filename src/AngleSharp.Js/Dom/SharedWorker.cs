namespace AngleSharp.Js.Dom
{
    using AngleSharp.Attributes;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Io;
    using AngleSharp.Js;
    using AngleSharp.Scripting;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a shared worker as seen from a window: the windows creating one for the same
    /// script (and name) of an origin share it, each talking to it through a port of its own,
    /// which the worker gets by a connect event.
    /// </summary>
    [DomName("SharedWorker")]
    [DomExposed("Window")]
    public sealed class SharedWorker : EventTarget
    {
        #region Fields

        private static readonly Dictionary<Tuple<JsScriptingService, String, String, String>, Scope> Scopes = new Dictionary<Tuple<JsScriptingService, String, String, String>, Scope>();

        private readonly MessagePort _port;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new shared worker, which connects to the running one for the script, or
        /// starts it in a separate context.
        /// </summary>
        /// <param name="window">The window creating the worker.</param>
        /// <param name="source">The address of the script.</param>
        public SharedWorker(IWindow window, String source)
            : this(window, source, null)
        {
        }

        /// <summary>
        /// Creates a new shared worker, which connects to the running one for the script, or
        /// starts it in a separate context.
        /// </summary>
        /// <param name="window">The window creating the worker.</param>
        /// <param name="source">The address of the script.</param>
        /// <param name="options">The name of the worker, or its options.</param>
        [DomConstructor]
        public SharedWorker(IWindow window, String source, Object options)
        {
            if (window?.Document == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            if (String.IsNullOrEmpty(source))
            {
                throw new DomException(DomError.Syntax);
            }

            var document = window.Document;
            var scripting = document.Context.GetService<JsScriptingService>() ?? throw new DomException(DomError.NotSupported);
            var url = Worker.ResolveUrl(document, source);
            var name = options is WorkerOptions workerOptions ? workerOptions.Name ?? String.Empty : options?.ToString() ?? String.Empty;
            var key = Tuple.Create(scripting, document.Origin ?? String.Empty, url.Href, name);

            // the port of the window, and the one of the worker, which gets its document once started
            var gate = new Object();
            _port = new MessagePort(document, gate);
            var inside = new MessagePort(null, gate);
            _port.Entangle(inside);

            Scope scope;

            lock (Scopes)
            {
                if (!Scopes.TryGetValue(key, out scope) || scope.IsClosed)
                {
                    scope = new Scope(key, document, scripting, url);
                    Scopes[key] = scope;
                }

                scope.Attach(window, this, inside);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the port to talk to the worker.
        /// </summary>
        [DomName("port")]
        public MessagePort Port => _port;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the script of the worker could not be started.
        /// </summary>
        [DomName("onerror")]
        public event DomEventHandler Failed
        {
            add { AddEventListener(EventNames.Error, value); }
            remove { RemoveEventListener(EventNames.Error, value); }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// The running script of a shared worker, with the windows connected to it.
        /// </summary>
        private sealed class Scope
        {
            private const String WorkerIncomingPortsName = "__anglesharpIncomingPorts";

            private const String ConnectScript = @"(function () {
var ev = { type: 'connect', data: '', origin: '', lastEventId: '', source: __anglesharpIncomingPorts[0], ports: __anglesharpIncomingPorts };
if (self && typeof self.onconnect === 'function') {
    self.onconnect(ev);
}
})();";

            private readonly Tuple<JsScriptingService, String, String, String> _key;
            private readonly JsScriptingService _scripting;
            private readonly IEventLoop _loop;
            private readonly IBrowsingContext _context;
            private readonly List<IWindow> _windows = new List<IWindow>();
            private readonly Object _gate = new Object();
            private List<Connection> _pending = new List<Connection>();
            private IDocument _document;
            private Exception _startupError;

            public Scope(Tuple<JsScriptingService, String, String, String> key, IDocument creator, JsScriptingService scripting, Url url)
            {
                _key = key;
                _scripting = scripting;
                _loop = new JsEventLoop();
                _context = Worker.CreateContext(creator.Context, scripting, _loop);

                //  The fetch of the script is asynchronous in a browser, so the worker cannot run
                //  before the script creating it is done: whatever else that script connects (a
                //  second SharedWorker for the same script) is connected before the worker sees a
                //  message. Here the script arrives synchronously on a thread of its own, so it is
                //  the start that waits instead - for the lock the creating script holds while it
                //  runs, whether it runs as a task of the event loop or was called from outside.
                var creatorEngine = scripting.GetOrCreateInstance(creator).Jint;
                var creatorLoop = creator.Context.GetService<IEventLoop>();
                Worker.Enqueue(creatorLoop, TaskPriority.Normal, () =>
                {
                    lock (creatorEngine)
                    {
                        //  the window may have gone in the meantime, and the worker with it
                        if (!IsClosed)
                        {
                            Worker.Enqueue(_loop, TaskPriority.Critical, () => Start(creator, url));
                        }
                    }
                });
            }

            public Boolean IsClosed { get; private set; }

            public void Attach(IWindow window, SharedWorker worker, MessagePort port)
            {
                if (!_windows.Contains(window))
                {
                    _windows.Add(window);
                    window.Unloaded += OnUnloaded;
                }

                var connection = new Connection(window, worker, port);

                lock (_gate)
                {
                    //  until the script ran there is nothing to connect to, nor an error to tell
                    if (_pending != null)
                    {
                        _pending.Add(connection);
                        return;
                    }
                }

                Worker.Enqueue(_loop, TaskPriority.Normal, () => Connect(connection));
            }

            private void Start(IDocument creator, Url url)
            {
                try
                {
                    _document = _context.OpenAsync(request => request.Address(url).Content("<!doctype html>")).GetAwaiter().GetResult();
                    var source = Worker.FetchScript(creator, url);
                    _scripting.EvaluateScript(_document, Worker.WorkerBootstrap, MimeTypeNames.DefaultJavaScript, url.Href);
                    _scripting.EvaluateScript(_document, source, MimeTypeNames.DefaultJavaScript, url.Href);
                }
                catch (Exception ex)
                {
                    _startupError = ex;
                }

                List<Connection> pending;

                lock (_gate)
                {
                    pending = _pending;
                    _pending = null;
                }

                //  all within this task, so that a message of the first port is delivered after
                //  the last of them is connected
                foreach (var connection in pending)
                {
                    Connect(connection);
                }
            }

            private void Connect(Connection connection)
            {
                if (IsClosed)
                {
                    return;
                }

                // the worker failing to start is told to every window connecting
                if (_startupError != null)
                {
                    var worker = connection.Worker;
                    var document = connection.Window.Document;

                    if (document == null)
                    {
                        return;
                    }

                    MessageTaskQueue.Enqueue(document, () =>
                    {
                        var ev = new ErrorEvent();
                        ev.Init(_key.Item3, 0, 0, _startupError);
                        worker.Dispatch(ev);
                    });
                    return;
                }

                var port = connection.Port;
                port.SetDocument(_document);
                var instance = _scripting.GetOrCreateInstance(_document);
                var ports = new[] { port };
                instance.Jint.SetValue(WorkerIncomingPortsName, ports.ToJsValue(instance));
                _document.DefaultView?.Dispatch(new MessageEvent("connect", false, false, String.Empty, String.Empty, String.Empty, null, ports));
                _scripting.EvaluateScript(_document, ConnectScript, MimeTypeNames.DefaultJavaScript, _document.Url);
            }

            private void OnUnloaded(Object sender, Event ev)
            {
                lock (Scopes)
                {
                    if (sender is IWindow window)
                    {
                        window.Unloaded -= OnUnloaded;
                        _windows.Remove(window);
                    }

                    // the worker ends with the last window it was created for
                    if (_windows.Count == 0 && !IsClosed)
                    {
                        IsClosed = true;
                        Scopes.Remove(_key);
                        ((IDisposable)_loop).Dispose();
                    }
                }
            }

            private sealed class Connection
            {
                public Connection(IWindow window, SharedWorker worker, MessagePort port)
                {
                    Window = window;
                    Worker = worker;
                    Port = port;
                }

                public IWindow Window { get; }

                public SharedWorker Worker { get; }

                public MessagePort Port { get; }
            }
        }

        #endregion
    }
}
