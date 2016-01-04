namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Runtime.Environments;
    using System;
    using System.Collections.Generic;

    sealed class EngineInstance
    {
        readonly Dictionary<Object, DomNodeInstance> _objects;
        readonly Engine _engine;
        readonly LexicalEnvironment _lexicals;
        readonly LexicalEnvironment _variables;
        readonly DomNodeInstance _window;
        readonly DomConstructors _constructors;

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments)
        {
            _objects = new Dictionary<Object, DomNodeInstance>();
            _engine = new Engine();
            _engine.SetValue("console", new ConsoleInstance(_engine));

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
    }
}
