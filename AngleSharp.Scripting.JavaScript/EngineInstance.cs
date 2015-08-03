namespace AngleSharp.Scripting.JavaScript
{
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
        readonly DomNodeInstance _this;
        readonly DomConstructors _constructors;

        public EngineInstance(Object @this)
        {
            _objects = new Dictionary<Object, DomNodeInstance>();
            _engine = new Engine();
            _engine.SetValue("console", new ConsoleInstance(_engine));
            _this = GetDomNode(@this);
            _lexicals = LexicalEnvironment.NewObjectEnvironment(_engine, _this, _engine.ExecutionContext.LexicalEnvironment, true);
            _variables = LexicalEnvironment.NewObjectEnvironment(_engine, _engine.Global, null, false);
            _constructors = new DomConstructors(this);
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

            if (_objects.TryGetValue(obj, out domNodeInstance) == false)
                _objects.Add(obj, domNodeInstance = new DomNodeInstance(_engine, obj));
            
            return domNodeInstance;
        }

        public void RunScript(String source)
        {
            _engine.EnterExecutionContext(Lexicals, Variables, _this);
            _engine.Execute(source);
            _engine.LeaveExecutionContext();
        }
    }
}
