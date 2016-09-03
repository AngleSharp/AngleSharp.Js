namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime.Environments;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class EngineInstance
    {
        #region Fields

        private readonly Dictionary<Object, DomNodeInstance> _objects;
        private readonly Engine _engine;
        private readonly LexicalEnvironment _lexicals;
        private readonly LexicalEnvironment _variables;
        private readonly DomNodeInstance _window;
        private readonly DomConstructors _constructors;

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments)
        {
            var logger = default(IConsoleLogger);
            var context = window.Document.Context;
            var createLogger = context.Configuration.Services.OfType<Func<IBrowsingContext, IConsoleLogger>>().FirstOrDefault();

            if (createLogger != null)
            {
                logger = createLogger.Invoke(context);
            }
            
            _objects = new Dictionary<Object, DomNodeInstance>();
            _engine = new Engine();
            _engine.SetValue("console", new ConsoleInstance(_engine, logger));

            foreach (var assignment in assignments)
            {
                _engine.SetValue(assignment.Key, assignment.Value);
            }

            _window = GetDomNode(window);
            _lexicals = LexicalEnvironment.NewObjectEnvironment(_engine, _window, _engine.ExecutionContext.LexicalEnvironment, true);
            _variables = LexicalEnvironment.NewObjectEnvironment(_engine, _engine.Global, null, false);
            _constructors = new DomConstructors(this);
            _constructors.Configure();

            this.AddConstructors(_window, typeof(INode));
            this.AddConstructors(_window, this.GetType());
            this.AddInstances(_window, this.GetType());
        }

        #endregion

        #region Properties

        public DomNodeInstance Window
        {
            get { return _window; }
        }

        public DomConstructors Constructors
        {
            get { return _constructors; }
        }
        
        public LexicalEnvironment Lexicals 
        {
            get { return _lexicals; }
        }

        public LexicalEnvironment Variables 
        {
            get { return _variables; }
        }

        public Engine Jint
        {
            get { return _engine; }
        }

        #endregion

        #region Methods

        public DomNodeInstance GetDomNode(Object obj)
        {
            var domNodeInstance = default(DomNodeInstance);

            if (!_objects.TryGetValue(obj, out domNodeInstance))
            {
                _objects.Add(obj, domNodeInstance = new DomNodeInstance(this, obj));
            }
            
            return domNodeInstance;
        }

        public void RunScript(String source)
        {
            _engine.EnterExecutionContext(Lexicals, Variables, _window);
            _engine.Execute(source);
            _engine.LeaveExecutionContext();
        }

        #endregion
    }
}
