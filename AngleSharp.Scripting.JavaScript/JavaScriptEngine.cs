namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using AngleSharp.Services.Scripting;
    using Jint;
    using Jint.Runtime.Environments;
    using System;
    using System.IO;
    using System.Text;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The JavaScript engine.
    /// </summary>
    public class JavaScriptEngine : IScriptEngine
    {
        static readonly ConditionalWeakTable<Engine, JavaScriptEngine> _engineLookup = new ConditionalWeakTable<Engine, JavaScriptEngine>();
        static readonly ConditionalWeakTable<IWindow, ContextCache> _contextCaches = new ConditionalWeakTable<IWindow, ContextCache>();
        static readonly object _padlock = new object();

        readonly Engine _engine;
        IWindow _windowContext;
        ContextCache _contextCache;

        internal static JavaScriptEngine Get(Engine engine)
        {
            //  Global so needs to be thread safe
            lock (_padlock)
            {
                JavaScriptEngine javaScriptEngine;

                if(_engineLookup.TryGetValue(engine, out javaScriptEngine) == false)
                    throw new Exception("Failed to get corresponsing javascript engine!");

                return javaScriptEngine;
            }
        }

        /// <summary>
        /// Creates a new JavaScript engine.s
        /// </summary>
        public JavaScriptEngine()
        {
            _engine = new Engine();
            _engine.SetValue("console", new ConsoleInstance(_engine));

            lock (_padlock)
                _engineLookup.Add(_engine, this);
        }

        /// <summary>
        /// Gets the engine's mime-type.
        /// </summary>
        public String Type
        {
            get { return MimeTypes.DefaultJavaScript; }
        }

        /// <summary>
        /// Gets the result of the previous evaluation.
        /// </summary>
        public Object Result
        {
            get { return _engine.GetCompletionValue(); }
        }

        /// <summary>
        /// Evaluates the given source.
        /// </summary>
        /// <param name="source">The source code to evaluate.</param>
        /// <param name="options">The options to consider.</param>
        public void Evaluate(String source, ScriptOptions options)
        {
            var objectContext = _windowContext = options.Context;
            ContextCache contextCache;
            DomNodeInstance domContext;

            lock (_padlock)
            {
                if (_contextCaches.TryGetValue(objectContext, out contextCache) == false)
                {
                    contextCache = new ContextCache();
                    _contextCaches.Add(objectContext, contextCache);
                }

                domContext = GetDomNodeInstance(objectContext);

                //  Get or create the new environment
                if (contextCache.LexicalEnvironment == null)
                {
                    contextCache.LexicalEnvironment = LexicalEnvironment.NewObjectEnvironment(_engine, domContext, _engine.ExecutionContext.LexicalEnvironment, true);
                    contextCache.VariableEnvironment = LexicalEnvironment.NewObjectEnvironment(_engine, _engine.Global, null, false);
                }
            }

            _engine.EnterExecutionContext(contextCache.LexicalEnvironment, contextCache.VariableEnvironment, domContext);
            _engine.Execute(source);
            _engine.LeaveExecutionContext();
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

        /// <summary>
        /// Gets a dom node instance from the cache, or creates it if it doesn't already exist
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal DomNodeInstance GetDomNodeInstance(object obj)
        {
            ContextCache contextCache;

            if (_contextCaches.TryGetValue(_windowContext, out contextCache) == false)
                throw new Exception("Failed to get context cache!");

            return contextCache.GetDomNodeInstance(_engine, obj);
        }

        /// <summary>
        /// Resets the engine.
        /// </summary>
        public void Reset()
        {
            //TODO Jint
        }
    }
}
