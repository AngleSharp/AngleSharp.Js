namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using Jint;
    using Jint.Runtime.Environments;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The JavaScript engine.
    /// </summary>
    public class JavaScriptEngine : IScriptEngine
    {
        readonly Engine _engine;
        readonly LexicalEnvironment _variable;

        /// <summary>
        /// Creates a new JavaScript engine.s
        /// </summary>
        public JavaScriptEngine()
        {
            _engine = new Engine();
            _engine.SetValue("console", new ConsoleInstance(_engine));
            _variable = LexicalEnvironment.NewObjectEnvironment(_engine, _engine.Global, null, false);
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
            var context = new DomNodeInstance(_engine, options.Context);
            var env = LexicalEnvironment.NewObjectEnvironment(_engine, context, _engine.ExecutionContext.LexicalEnvironment, true);
            _engine.EnterExecutionContext(env, _variable, context);
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
        /// Resets the engine.
        /// </summary>
        public void Reset()
        {
            //TODO Jint
        }
    }
}
