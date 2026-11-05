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
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    sealed class EngineInstance
    {
        #region Fields

        //  Jint's StackGuard.Disabled, which is internal.
        private const Int32 StackGuardDisabled = -1;

        private readonly Engine _engine;
        private readonly TimeSpan _regexTimeout;
        private readonly PrototypeCache _prototypes;
        private readonly ReferenceCache _references;
        private readonly ConditionalWeakTable<Object, SameObjectCache> _sameObjects;
        private readonly LibrarySet _libs;
        private readonly DomNodeInstance _window;
        private readonly IDocument _document;
        private readonly JsImportMap _importMap;
        private readonly List<KeyValuePair<JsValue, JsValue>> _unhandledRejections = new List<KeyValuePair<JsValue, JsValue>>();

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments, IEnumerable<Assembly> libs, JsScriptingOptions options)
        {
            _importMap = new JsImportMap();
            var engineOptions = new Options();
            engineOptions.EnableModules(new JsModuleLoader(this, window.Document));
            // The handler uses the caches initialized below. The creator must return the
            // engine before DOM objects are wrapped or page scripts are evaluated.
            engineOptions.SetWrapObjectHandler(WrapObject);
            // Left alone, the JS call stack is the native one, and a runaway recursion
            // takes the process down with an uncatchable StackOverflowException.
            engineOptions.Constraints.MaxExecutionStackCount = options.MaxCallStackDepth > 0 ? options.MaxCallStackDepth : StackGuardDisabled;
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
                    var error = completed.Exception.GetBaseException();
                    reject(_engine.Intrinsics.Error.Construct(error.Message));
                }
                else if (completed.IsCanceled)
                {
                    reject(_engine.Intrinsics.Error.Construct("The operation was aborted."));
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
                    NotifyUnhandledRejection(rejection.Key, rejection.Value);
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

        private void Settle(Action settle)
        {
            void Run()
            {
                lock (_engine)
                {
                    settle();
                    _engine.Advanced.ProcessTasks();
                }
            }

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
                return cache.GetOrUpdate(getter, current, () => CreateInstance(current, getter.ReturnType));
            }
            catch (TargetInvocationException)
            {
                throw new JavaScriptException(_engine.Intrinsics.Error);
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
        /// Reports an error nobody caught (HTML 8.1.4.7): the window gets to handle it, otherwise the
        /// browsing context gets to know of it. An error thrown while the window handles one isn't
        /// reported to the window as well, as that would never end.
        /// </summary>
        public void ReportError(JavaScriptException error)
        {
            var window = (IWindow)_window.Value;

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
            if (string.IsNullOrEmpty(type))
            {
                type = MimeTypeNames.DefaultJavaScript;
            }

            lock (_engine)
            {
                if (MimeTypeNames.IsJavaScript(type))
                {
                    var prepared = ScriptCache.GetOrCreate(source, sourceUrl, position, _regexTimeout);
                    //  An invalid result means the source did not parse; hand it to the
                    //  engine as text so the syntax error is reported as usual.
                    return prepared.IsValid ? _engine.Evaluate(prepared) : _engine.Evaluate(source, sourceUrl, new ScriptParsingOptions { SourceOffset = position, RegexTimeout = _regexTimeout });
                }
                else if (type.Isi("importmap"))
                {
                    return LoadImportMap(source);
                }
                else if (type.Isi("module"))
                {
                    // use a unique specifier to import the module into Jint
                    var specifier = sourceUrl ?? Guid.NewGuid().ToString();

                    return ImportModule(specifier, source);
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

        private JsValue ImportModule(String specifier, String source)
        {
            _engine.Modules.Add(specifier, source);
            _engine.Modules.Import(specifier);

            return JsValue.Undefined;
        }

        #endregion

        #region Helpers

        //  The members of the window are configurable (and its operations and interface objects
        //  writable) per WebIDL, so that a page's own global replaces them as in a browser, e.g.
        //  var item = ..., var open = ... or var Node = ...; an accessor like name keeps its setter.
        private static PropertyDescriptor AsGlobalProperty(PropertyDescriptor descriptor) =>
            descriptor.IsAccessorDescriptor()
                ? new GetSetPropertyDescriptor(descriptor.Get, descriptor.Set, descriptor.Enumerable, true)
                : new PropertyDescriptor(descriptor.Value, true, descriptor.Enumerable, true);

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
