namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using AngleSharp.Services.Scripting;
    using Jint;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// The JavaScript engine.
    /// </summary>
    public class JavaScriptEngine : IScriptEngine
    {
        readonly ConditionalWeakTable<IWindow, EngineInstance> _contexts;
        readonly ConditionalWeakTable<Engine, EngineInstance> _engines;

        /// <summary>
        /// Creates a new JavaScript engine.
        /// </summary>
        JavaScriptEngine()
        {
            _contexts = new ConditionalWeakTable<IWindow, EngineInstance>();
            _engines = new ConditionalWeakTable<Engine, EngineInstance>();
        }

        /// <summary>
        /// The instance of the JavaScript engine.
        /// </summary>
        public static readonly JavaScriptEngine Instance = new JavaScriptEngine();

        /// <summary>
        /// Gets the engine's mime-type.
        /// </summary>
        public String Type
        {
            get { return MimeTypes.DefaultJavaScript; }
        }

        /// <summary>
        /// Evaluates the given source.
        /// </summary>
        /// <param name="source">The source code to evaluate.</param>
        /// <param name="options">The options to consider.</param>
        public void Evaluate(String source, ScriptOptions options)
        {
            var objectContext = options.Context;
            var instance = default(EngineInstance);

            if (_contexts.TryGetValue(objectContext, out instance) == false)
            {
                _contexts.Add(objectContext, instance = new EngineInstance(objectContext));
                _engines.Add(instance.Jint, instance);
            }

            instance.RunScript(source);
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
        /// Gets a context cache for the provided engine.
        /// </summary>
        internal EngineInstance GetCache(Engine engine)
        {
            var instance = default(EngineInstance);
            _engines.TryGetValue(engine, out instance);
            return instance;
        }
    }
}
