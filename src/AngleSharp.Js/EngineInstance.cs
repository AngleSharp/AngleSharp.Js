namespace AngleSharp.Js
{
    using Acornima;
    using AngleSharp.Browser;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Io;
    using AngleSharp.Js.Cache;
    using AngleSharp.Js.Dom;
    using AngleSharp.Text;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Json;
    using Jint.Native.Object;
    using Jint.Native.Promise;
    using Jint.Native.Symbol;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// An element whose constructor is running.
    /// </summary>
    sealed class CustomElementConstruction
    {
        public CustomElementConstruction(IElement element, CustomElementDefinition definition)
        {
            Element = element;
            Definition = definition;
        }

        public IElement Element { get; }

        public CustomElementDefinition Definition { get; }

        public Boolean IsDone { get; set; }
    }

    sealed class EngineInstance
    {
        #region Fields

        private Int32 _scriptDepth;
        private const String ScriptValueKey = "AngleSharp.ScriptValue";

        //  Jint's StackGuard.Disabled, which is internal.
        private const Int32 StackGuardDisabled = -1;

        private readonly Engine _engine;
        private readonly TimeSpan _regexTimeout;
        private readonly Boolean _retainFunctionSourceText;
        private readonly PrototypeCache _prototypes;
        private readonly ReferenceCache _references;
        private readonly ConditionalWeakTable<Object, SameObjectCache> _sameObjects;
        private readonly LibrarySet _libs;
        private readonly DomNodeInstance _window;
        private readonly IDocument _document;
        private readonly JsImportMap _importMap;
        private readonly ConditionalWeakTable<DomException, JsValue> _domExceptions = new ConditionalWeakTable<DomException, JsValue>();
        private readonly Stack<CustomElementConstruction> _customElementConstructions = new Stack<CustomElementConstruction>();
        private JsValue _domException;
        private JsValue _lastReportedError;
        private Int32 _inlineModules;
        private readonly List<KeyValuePair<JsValue, JsValue>> _unhandledRejections = new List<KeyValuePair<JsValue, JsValue>>();

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments, IEnumerable<Assembly> libs, JsScriptingOptions options)
        {
            _importMap = new JsImportMap();
            var engineOptions = new Options();
            engineOptions.EnableModules(new JsModuleLoader(this, window.Document));
            engineOptions.UseHostFactory(_ => new JsHost(window.Document));
            // The handler uses the caches initialized below. The creator must return the
            // engine before DOM objects are wrapped or page scripts are evaluated.
            engineOptions.SetWrapObjectHandler(WrapObject);
            // Left alone, the JS call stack is the native one, and a runaway recursion
            // takes the process down with an uncatchable StackOverflowException.
            engineOptions.Constraints.MaxExecutionStackCount = options.MaxCallStackDepth > 0 ? options.MaxCallStackDepth : StackGuardDisabled;
            // for what the engine parses itself (eval, new Function, modules)
            engineOptions.RetainFunctionSourceText = options.RetainFunctionSourceText;
            _retainFunctionSourceText = options.RetainFunctionSourceText;
            var creator = window.Document.Context.GetService<EngineCreator>();
            _engine = (creator is null ? new Engine(engineOptions) : creator.Invoke(window, engineOptions))
                ?? throw new InvalidOperationException("The engine creator must return a Jint engine.");
            _regexTimeout = engineOptions.Constraints.RegexTimeout;
            _libs = new LibrarySet(libs);
            _prototypes = new PrototypeCache(_engine, _libs);
            _references = new ReferenceCache();
            _sameObjects = new ConditionalWeakTable<Object, SameObjectCache>();

            foreach (var assignment in assignments)
            {
                _engine.SetValue(assignment.Key, assignment.Value);
            }

            _self = new WeakReference<EngineInstance>(this);
            _document = window.Document;
            _window = (DomNodeInstance)GetDomNode(window);

            foreach (var lib in libs)
            {
                this.AddConstructors(_window, lib);
                this.AddConstructorFunctions(_window, lib);
                this.AddInstances(_window, lib);
            }

            _window.FastSetProperty("DOMException", CreateDomExceptionProperty());

            foreach (var property in _window.GetOwnProperties())
            {
                _engine.Global.FastSetProperty(property.Key.ToString(), AsGlobalProperty(property.Value));
            }

            foreach (var prototypeProperty in Window.Prototype.GetOwnProperties())
            {
                _engine.Global.FastSetProperty(prototypeProperty.Key.ToString(), AsGlobalProperty(prototypeProperty.Value));
            }

            _engine.Global.Prototype = _window.Prototype;
            _engine.Advanced.PromiseRejectionTracker += OnPromiseRejection;
        }

        #endregion

        #region Properties

        public IEnumerable<Assembly> Libs => _libs.Assemblies;

        public DomNodeInstance Window => _window;

        /// <summary>
        /// Gets the document the engine runs the scripts of.
        /// </summary>
        public IDocument Document => _document;

        /// <summary>
        /// The engine of the script that called into the DOM last on this thread.
        /// </summary>
        public static EngineInstance Current
        {
            get => s_current != null && s_current.TryGetTarget(out var engine) ? engine : null;
            set => s_current = value?._self;
        }

        // weak, as an idle thread must not keep the engine of a closed page alive
        [ThreadStatic]
        private static WeakReference<EngineInstance> s_current;
        private readonly WeakReference<EngineInstance> _self;
        private Boolean _isReportingError;

        /// <summary>
        /// Whether the node is of a document of another origin, which script here must not get.
        /// </summary>
        public Boolean IsOutOfReach(Object obj) =>
            obj is INode node && OriginPolicy.IsCrossOrigin(_document, node as IDocument ?? node.Owner);

        public Engine Jint => _engine;

        public JsImportMap ImportMap => _importMap;

        #endregion

        #region Methods

        /// <summary>
        /// Gives script a promise for the task (e.g. of fetch), settled with the outcome of the
        /// task on the event loop of the window - Jint is not thread-safe, and its promise jobs
        /// only run when asked to.
        /// </summary>
        public JsValue CreatePromise(Task task)
        {
            var (promise, resolve, reject) = _engine.Advanced.RegisterPromise();
            var hasResult = GetResultType(task.GetType()) != null;

            task.ContinueWith(completed => Settle(() =>
            {
                if (completed.IsFaulted)
                {
                    reject(ToScriptValue(completed.Exception.GetBaseException()));
                }
                else if (completed.IsCanceled)
                {
                    reject(GetDomException(new DomException(DomError.Abort)));
                }
                else if (hasResult)
                {
                    var result = completed.GetType().GetProperty(nameof(Task<Object>.Result)).GetValue(completed);
                    //  untyped, so a node is the same object as the one the DOM gives otherwise
                    resolve(result.ToJsValue(this));
                }
                else
                {
                    resolve(JsValue.Undefined);
                }
            }), TaskContinuationOptions.ExecuteSynchronously);

            return promise;
        }

        //  A promise rejected without a handler is reported once the task rejecting it is done,
        //  which gives the script the rest of it to add one (HTML 8.1.7.3); without an event loop
        //  there is no end of a task to wait for.
        private void OnPromiseRejection(Object sender, PromiseRejectionTrackerEventArgs e)
        {
            var loop = GetEventLoop();

            if (loop == null)
            {
                return;
            }

            var index = _unhandledRejections.FindIndex(rejection => ReferenceEquals(rejection.Key, e.Promise));

            if (e.Operation == PromiseRejectionOperation.Handle)
            {
                if (index >= 0)
                {
                    _unhandledRejections.RemoveAt(index);
                }
            }
            else if (index < 0)
            {
                _unhandledRejections.Add(new KeyValuePair<JsValue, JsValue>(e.Promise, e.Value));

                if (_unhandledRejections.Count == 1)
                {
                    loop.Enqueue(_ => NotifyUnhandledRejections(), TaskPriority.Normal);
                }
            }
        }

        private void NotifyUnhandledRejections()
        {
            lock (_engine)
            {
                var rejections = _unhandledRejections.ToArray();
                _unhandledRejections.Clear();

                foreach (var rejection in rejections)
                {
                    //  an error in a module also rejects the promise of its evaluation
                    if (!ReferenceEquals(rejection.Value, _lastReportedError))
                    {
                        NotifyUnhandledRejection(rejection.Key, rejection.Value);
                    }
                }
            }
        }

        private void NotifyUnhandledRejection(JsValue promise, JsValue reason)
        {
            var window = (IWindow)_window.Value;
            var ev = new PromiseRejectionEvent("unhandledrejection", promise, reason);
            window.Dispatch(ev);

            //  the handler a script assigned to the window, which is no member of the DOM
            if (!ev.IsDefaultPrevented && _window.Get("onunhandledrejection").AsFunction(this) is Function handler)
            {
                var result = handler.Call(_window, new JsValue[] { GetDomNode(ev) });

                if (result.IsBoolean() && !result.AsBoolean())
                {
                    ev.Cancel();
                }
            }

            if (!ev.IsDefaultPrevented)
            {
                window.Document?.Context?.TrackError(new JavaScriptException(reason));
            }
        }

        /// <summary>
        /// Runs what script gave as a callback whose result is waited for (e.g. of a lock). It is
        /// called from anywhere, so it runs on the event loop as the engine isn't thread-safe, and
        /// the task follows the promise the function may give.
        /// </summary>
        public Task<Object> RunCallback(Function function, Object receiver, Object[] arguments)
        {
            var completion = new TaskCompletionSource<Object>(TaskCreationOptions.RunContinuationsAsynchronously);

            Settle(() =>
            {
                try
                {
                    var values = new JsValue[arguments.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = arguments[i].ToJsValue(this);
                    }

                    var result = function.Call(receiver.ToJsValue(this), values);
                    var then = result is ObjectInstance candidate ? candidate.Get("then") : JsValue.Undefined;

                    if (then.IsCallable())
                    {
                        _engine.Call(then, result, new JsValue[]
                        {
                            new ClrFunction(_engine, "resolve", (_, values) =>
                            {
                                completion.TrySetResult(values.Length > 0 ? values[0] : JsValue.Undefined);
                                return JsValue.Undefined;
                            }),
                            new ClrFunction(_engine, "reject", (_, values) =>
                            {
                                completion.TrySetException(new JavaScriptException(values.Length > 0 ? values[0] : JsValue.Undefined));
                                return JsValue.Undefined;
                            }),
                        });
                    }
                    else
                    {
                        completion.TrySetResult(result);
                    }
                }
                catch (JavaScriptException thrown)
                {
                    completion.TrySetException(thrown);
                }
            });

            return completion.Task;
        }

        /// <summary>
        /// Tells that the host is about to run script, e.g. a timer or a listener.
        /// </summary>
        public void EnterScript() => _scriptDepth++;

        /// <summary>
        /// Tells that the host ran script. When no other script is running anymore the promise
        /// jobs it left are due, which is the microtask checkpoint of HTML 8.1.4.4. Script calling
        /// into the DOM, e.g. by dispatchEvent, keeps them waiting until it is done itself.
        /// </summary>
        public void LeaveScript()
        {
            if (_scriptDepth == 1)
            {
                lock (_engine)
                {
                    try
                    {
                        _engine.Advanced.ProcessTasks();
                    }
                    catch (JavaScriptException thrown)
                    {
                        ReportError(thrown);
                    }
                }
            }

            _scriptDepth--;
        }

        internal void RunTask(Action action)
        {
            var previous = Current;

            try
            {
                lock (_engine)
                {
                    Current = this;
                    EnterScript();

                    try
                    {
                        action();
                    }
                    finally
                    {
                        LeaveScript();
                    }
                }
            }
            finally
            {
                Current = previous;
            }
        }

        private void Settle(Action settle)
        {
            void Run() => RunTask(settle);

            var loop = GetEventLoop();

            if (loop != null)
            {
                loop.Enqueue(_ => Run(), TaskPriority.Normal);
            }
            else
            {
                Run();
            }
        }

        private IEventLoop GetEventLoop() => (_window.Value as IWindow)?.Document?.Context?.GetService<IEventLoop>();

        //  The result of a Task<T> (an async method gives a subclass of it); none for a Task, and
        //  none for the internal VoidTaskResult of a Task completed without a value.
        private static Type GetResultType(Type type)
        {
            for (; type != null && type != typeof(Task); type = type.GetTypeInfo().BaseType)
            {
                if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var result = type.GetTypeInfo().GenericTypeArguments[0];
                    return result.Name == "VoidTaskResult" ? null : result;
                }
            }

            return null;
        }

        public ObjectInstance GetDomNode(Object obj) => _references.GetOrCreate(obj, CreateInstance);

        public ObjectInstance GetDomNode(Object obj, Type type) =>
            obj is IWindow || IsOutOfReach(obj) ? GetDomNode(obj) : CreateInstance(obj, type);

        public ObjectInstance GetDomPrototype(Type type) => _prototypes.GetOrCreate(type, CreatePrototype);

        public JsValue GetSameObject(Object owner, MethodInfo getter)
        {
            if (getter == null)
            {
                return JsValue.Undefined;
            }

            try
            {
                var current = getter.Invoke(owner, Array.Empty<Object>());

                if (current == null)
                {
                    return JsValue.Null;
                }

                // Nodes and windows already have a canonical proxy. Preserve its identity,
                // concrete prototype and script-owned properties across SameObject getters.
                if (IsOutOfReach(current))
                {
                    return JsValue.Null;
                }

                if (current is INode || current is IWindow)
                {
                    return GetDomNode(current);
                }

                var cache = _sameObjects.GetValue(owner, _ => new SameObjectCache());

                //  The interface of a collection doesn't tell that it is one, the class does: script
                //  iterates over document.links or element.children like any other.
                var type = current is ITokenList || current.GetType().GetGenericInterface(typeof(IHtmlCollection<>)) != null ? current.GetType() : getter.ReturnType;
                return cache.GetOrUpdate(getter, current, () => CreateInstance(current, type));
            }
            catch (TargetInvocationException ex)
            {
                throw ToScriptError(ex);
            }
        }

        /// <summary>
        /// Gets the constructor object of the given type, building it on first ask. The
        /// prototype keeps it, so that naming the type and reading "constructor" off one of
        /// its instances arrive at the same object.
        /// </summary>
        public DomConstructorInstance GetDomConstructor(ConstructorDefinition definition)
        {
            //  Only the prototype of System.Object is not one of ours, and that type is not
            //  exposed as a constructor, so it never reaches this point.
            var prototype = GetDomPrototype(definition.Type);
            return DomPrototypeState.Of(prototype).GetConstructor(definition);
        }

        /// <summary>
        /// Starts running the constructor for an existing element: the first constructor of the DOM
        /// reached by it has to give that element.
        /// </summary>
        public CustomElementConstruction BeginCustomElementConstruction(IElement element, CustomElementDefinition definition)
        {
            var construction = new CustomElementConstruction(element, definition);
            _customElementConstructions.Push(construction);
            return construction;
        }

        public void EndCustomElementConstruction(CustomElementConstruction construction)
        {
            if (_customElementConstructions.Count > 0 && ReferenceEquals(_customElementConstructions.Peek(), construction))
            {
                _customElementConstructions.Pop();
            }
        }

        /// <summary>
        /// What a constructor of the DOM does for a class deriving from it (HTML 3.2.3): that is a
        /// custom element, or else nobody may create it. The element is the one being upgraded, or a
        /// new one for a script creating it with new, and it has the prototype of the class.
        /// </summary>
        public ObjectInstance ConstructCustomElement(JsValue newTarget)
        {
            var window = (IWindow)_window.Value;
            var definition = window.CustomElements().FindDefinition(newTarget)
                ?? throw new JavaScriptException(_engine.Intrinsics.TypeError, "Illegal constructor.");
            ObjectInstance wrapper;

            if (_customElementConstructions.Count > 0 && ReferenceEquals(_customElementConstructions.Peek().Definition, definition))
            {
                var construction = _customElementConstructions.Peek();

                if (construction.IsDone)
                {
                    throw new JavaScriptException(_engine.Intrinsics.TypeError, "The element has been constructed already.");
                }

                construction.IsDone = true;
                wrapper = GetDomNode(construction.Element);
            }
            else
            {
                wrapper = GetDomNode(window.CustomElements().CreateElement(definition));
            }

            if (newTarget.Get("prototype") is ObjectInstance prototype)
            {
                wrapper.Prototype = prototype;
            }

            return wrapper;
        }

        /// <summary>
        /// Gets what a script sees of a member of the DOM failing: what the member threw for it, a
        /// DOMException, or else an error with the message of what went wrong.
        /// </summary>
        public JavaScriptException ToScriptError(TargetInvocationException error)
        {
            return error.InnerException is JavaScriptException thrown
                ? thrown
                : new JavaScriptException(ToScriptValue(error.InnerException));
        }

        /// <summary>
        /// Gets what a script gets to see of an exception: what was thrown for it, a DOMException,
        /// or else an error with the message of what went wrong.
        /// </summary>
        public JsValue ToScriptValue(Exception error)
        {
            if (error is JavaScriptException thrown)
            {
                return thrown.Error;
            }

            //  an exception can't be just any value, so what wants to throw one puts it there
            if (error != null && error.Data.Contains(ScriptValueKey))
            {
                return error.Data[ScriptValueKey].ToJsValue(this);
            }

            return error is DomException exception
                ? GetDomException(exception)
                : _engine.Intrinsics.Error.Construct(error?.Message ?? String.Empty);
        }

        /// <summary>
        /// Gets the DOMException of a script for the exception, which is the same object each time.
        /// </summary>
        public JsValue GetDomException(DomException exception) =>
            _domExceptions.GetValue(exception, key => DomExceptions.Create(_engine, GetDomException(), key));

        /// <summary>
        /// Reports an error nobody caught (HTML 8.1.4.7): the window gets to handle it, otherwise the
        /// browsing context gets to know of it. An error thrown while the window handles one isn't
        /// reported to the window as well, as that would never end.
        /// </summary>
        public void ReportError(JavaScriptException error)
        {
            var window = (IWindow)_window.Value;
            _lastReportedError = error.Error;

            if (!_isReportingError)
            {
                var location = error.Location;
                var ev = new ErrorEvent();
                //  the versions of AngleSharp before 1.10 don't set the type below
                ev.Init(EventNames.Error, false, true);
                ev.Init(location.SourceFile, location.Start.Line, location.Start.Column, error);
                _isReportingError = true;

                try
                {
                    window.Dispatch(ev);
                }
                finally
                {
                    _isReportingError = false;
                }

                if (ev.IsDefaultPrevented)
                {
                    return;
                }
            }

            window.Document?.Context?.TrackError(error);
        }

        public JsValue RunScript(String source, String type, String sourceUrl) =>
            RunScript(source, type, sourceUrl, default);

        /// <summary>
        /// Runs the script, of which the position in what it is written in is given, if any.
        /// </summary>
        public JsValue RunScript(String source, String type, String sourceUrl, Position position)
        {
            lock (_engine)
            {
                EnterScript();

                try
                {
                    return RunScriptCore(source, type, sourceUrl, position);
                }
                finally
                {
                    LeaveScript();
                }
            }
        }

        private JsValue RunScriptCore(String source, String type, String sourceUrl, Position position)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = MimeTypeNames.DefaultJavaScript;
            }

            lock (_engine)
            {
                if (MimeTypeNames.IsJavaScript(type))
                {
                    var prepared = ScriptCache.GetOrCreate(source, sourceUrl, position, _regexTimeout, _retainFunctionSourceText);
                    //  An invalid result means the source did not parse; hand it to the
                    //  engine as text so the syntax error is reported as usual.
                    return prepared.IsValid ? _engine.Evaluate(prepared) : _engine.Evaluate(source, sourceUrl, new ScriptParsingOptions { SourceOffset = position, RegexTimeout = _regexTimeout, RetainFunctionSourceText = _retainFunctionSourceText });
                }
                else if (type.Isi("importmap"))
                {
                    return LoadImportMap(source);
                }
                else if (type.Isi("module"))
                {
                    // the specifier imports the module into Jint, so one without an address gets a unique one
                    var specifier = sourceUrl ?? CreateInlineSpecifier();

                    // Jint has no options for the position of a module, but only spaces go before
                    var padding = position.Line > 0 ? new String('\n', position.Line - 1) + new String(' ', position.Column) : String.Empty;

                    return ImportModule(specifier, padding + source);
                }
                else
                {
                    return JsValue.Undefined;
                }
            }
        }

        private JsValue LoadImportMap(String source)
        {
            //  The source is page content, so it must be handed to a JSON parser rather
            //  than pasted into a script: a single quote already breaks the parse, and
            //  anything after a closing quote would run as script.
            var importMap = new JsonParser(_engine).Parse(source).AsObject();

            if (importMap.TryGetValue("scopes", out var scopes))
            {
                var scopesObj = scopes.AsObject();

                foreach (var scopeProperty in scopesObj.GetOwnProperties())
                {
                    var scopePath = scopeProperty.Key.AsString();

                    if (_importMap.Scopes.ContainsKey(scopePath))
                    {
                        continue;
                    }

                    var scopeValue = new Dictionary<string, Uri>();

                    var scopeImports = scopesObj[scopePath].AsObject();

                    foreach (var scopeImportProperty in scopeImports.GetOwnProperties())
                    {
                        var scopeImportSpecifier = scopeImportProperty.Key.AsString();

                        if (!scopeValue.ContainsKey(scopeImportSpecifier))
                        {
                            scopeValue.Add(scopeImportSpecifier, new Uri(scopeImports[scopeImportSpecifier].AsString(), UriKind.RelativeOrAbsolute));
                        }
                    }

                    _importMap.Scopes.Add(scopePath, scopeValue);
                }
            }

            if (importMap.TryGetValue("imports", out var imports))
            {
                var importsObj = imports.AsObject();

                foreach (var importProperty in importsObj.GetOwnProperties())
                {
                    var importSpecifier = importProperty.Key.AsString();

                    if (!_importMap.Imports.ContainsKey(importSpecifier))
                    {
                        _importMap.Imports.Add(importSpecifier, new Uri(importsObj[importSpecifier].AsString(), UriKind.RelativeOrAbsolute));
                    }
                }
            }

            return JsValue.Undefined;
        }

        //  Most pages never get to see one, and a script counts against the constraints of an engine,
        //  which may be strict, so it is only run when asked for. A page may replace it, as any global.
        private PropertyDescriptor CreateDomExceptionProperty()
        {
            var replacement = default(JsValue);
            var isReplaced = false;
            var getter = new ClrFunction(_engine, "DOMException", (self, arguments) => isReplaced ? replacement : GetDomException());
            var setter = new ClrFunction(_engine, "DOMException", (self, arguments) =>
            {
                replacement = arguments.Length > 0 ? arguments[0] : JsValue.Undefined;
                isReplaced = true;
                return JsValue.Undefined;
            });
            return new GetSetPropertyDescriptor(getter, setter, false, true);
        }

        private JsValue GetDomException() => _domException ?? (_domException = DomExceptions.CreateConstructor(_engine));

        private String CreateInlineSpecifier()
        {
            var baseUri = _document.BaseUri;

            if (Uri.TryCreate(baseUri, UriKind.Absolute, out var uri) && uri.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https))
            {
                return $"{uri.AbsoluteUri}{JsHost.InlineModuleMark}{++_inlineModules}";
            }

            return Guid.NewGuid().ToString();
        }

        private JsValue ImportModule(String specifier, String source)
        {
            // what the loader resolves it to is what the module is found by
            if (Uri.TryCreate(specifier, UriKind.Absolute, out var uri) && uri.Scheme.IsOneOf(ProtocolNames.Http, ProtocolNames.Https))
            {
                specifier = uri.AbsoluteUri;
            }

            _engine.Modules.Add(specifier, source);
            _engine.Modules.Import(specifier);

            return JsValue.Undefined;
        }

        #endregion

        #region Helpers

        //  The members of the window are configurable (and its operations and interface objects
        //  writable) per WebIDL, so that a page's own global replaces them as in a browser, e.g.
        //  var item = ..., var open = ... or var Node = ...; an accessor like name keeps its setter.
        //  A descriptor that is fine as it is stays shared: window.Node = ... also changes Node.
        private static PropertyDescriptor AsGlobalProperty(PropertyDescriptor descriptor) =>
            descriptor.IsAccessorDescriptor()
                ? new GetSetPropertyDescriptor(descriptor.Get, descriptor.Set, descriptor.Enumerable, true)
                : descriptor.Writable && descriptor.Configurable
                    ? descriptor
                    : new PropertyDescriptor(descriptor.Value, true, descriptor.Enumerable, true);

        //  A WebIDL iterable<K, V> (e.g. Headers) is iterated like its entries, which a type giving
        //  entries has no other way to say. A collection is iterated by its proxy, see there.
        private void SetIterator(ObjectInstance prototype, DomShape shape)
        {
            if (shape.Members.ContainsKey("entries") && shape.Indexers == null && !typeof(IEnumerable).IsAssignableFrom(shape.Type))
            {
                prototype.FastSetProperty(GlobalSymbolRegistry.Iterator, new PropertyDescriptor(
                    new ClrFunction(_engine, "[Symbol.iterator]", (thisObject, arguments) =>
                    {
                        var entries = thisObject.AsObject().Get("entries");
                        var list = _engine.Call(entries, thisObject, Arguments.Empty);
                        return _engine.Call(list.AsObject().Get(GlobalSymbolRegistry.Iterator), list, Arguments.Empty);
                    }),
                    true, false, true));
            }
        }

        //  A collection is projected by the array-like proxy, which lets the engine read its
        //  indices and length directly; everything else by the ordinary one. The decision is a
        //  property of the type, so it is resolved once per type for the whole process.
        private ObjectInstance CreateInstance(Object obj)
        {
            // what got here from another origin is all the same denied, whoever asked for it
            if (obj is IWindow window && OriginPolicy.IsCrossOrigin(_document, window.Document))
            {
                return CreateInstance(new CrossOriginWindow(window, _document), typeof(CrossOriginWindow));
            }

            return IsOutOfReach(obj) ? new Jint.Native.JsObject(_engine) : CreateInstance(obj, obj.GetType());
        }

        private ObjectInstance CreateInstance(Object obj, Type type)
        {
            var collection = type.GetIndexedCollection();

            if (collection != null)
            {
                return new DomCollectionInstance(this, obj, type, collection);
            }

            return new DomNodeInstance(this, obj, type);
        }

        /// <summary>
        /// Builds this engine's prototype for a DOM type: an object over the member layout the
        /// whole process shares for that type, chained to the prototype of its base type and
        /// carrying what only this engine knows about it.
        /// </summary>
        private ObjectInstance CreatePrototype(Type type)
        {
            var shape = DomShapeCache.GetOrCreate(type, _libs, _engine);
            var prototype = shape.Shape.Instantiate(_engine, GetParentPrototype(type, shape.BaseType));
            JsObjectShape.SetHostState(prototype, new DomPrototypeState(this, prototype, shape));
            SetIterator(prototype, shape);
            return prototype;
        }

        //  The base type may fold onto this very prototype - a class carrying no DOM name of its
        //  own shares the one of its nearest named ancestor. Asking for it would then re-enter
        //  the cache entry currently being built, so the fold is ruled out before the ask and the
        //  chain stops at Object.prototype, which is where it used to stop as well.
        private ObjectInstance GetParentPrototype(Type type, Type baseType) =>
            ReferenceEquals(_prototypes.Canonicalize(baseType), type)
                ? _engine.Intrinsics.Object.PrototypeObject
                : GetDomPrototype(baseType);

        /// <summary>
        /// Converts a value handed over from C#, which reaches Jint through JsValue.FromObject
        /// rather than through <see cref="EngineExtensions.ToJsValue(Object, EngineInstance)"/>.
        /// </summary>
        /// <remarks>
        /// A DOM object has to arrive as the DOM proxy for the very same reason it does when the
        /// DOM itself yields one: the proxy is what carries the DOM members, and it is what makes
        /// the object script already holds and the one passed in from C# the same object. Anything
        /// else stays with Jint's CLR wrapper, which is what a host object is expected to be.
        /// </remarks>
        private ObjectInstance WrapObject(Engine engine, Object target, Type type) =>
            target.GetType().IsDomType() ? GetDomNode(target) : ObjectWrapper.Create(engine, target, type);

        private sealed class SameObjectCache
        {
            private readonly Dictionary<MethodInfo, ObjectInstance> _entries = new Dictionary<MethodInfo, ObjectInstance>();

            public ObjectInstance GetOrUpdate(MethodInfo getter, Object value, Func<ObjectInstance> create)
            {
                lock (_entries)
                {
                    if (!_entries.TryGetValue(getter, out var instance))
                    {
                        instance = create.Invoke();
                        _entries.Add(getter, instance);
                    }
                    else if (instance is IDomProxy proxy)
                    {
                        proxy.Update(value);
                    }

                    return instance;
                }
            }
        }

        #endregion
    }
}
