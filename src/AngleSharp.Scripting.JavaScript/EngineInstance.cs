namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Environments;
    using System;
    using System.Collections.Generic;

    sealed class EngineInstance
    {
        #region Fields

        private readonly PrototypeCache _prototypes;
        private readonly ReferenceCache _references;
        private readonly Engine _engine;
        private readonly LexicalEnvironment _lexicals;
        private readonly LexicalEnvironment _variables;
        private readonly DomNodeInstance _window;

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments)
        {
            var context = window.Document.Context;
            var logger = context.GetService<IConsoleLogger>();
            _engine = new Engine();
            _prototypes = new PrototypeCache(_engine);
            _references = new ReferenceCache();
            _engine.SetValue("console", new ConsoleInstance(_engine, logger));

            foreach (var assignment in assignments)
            {
                _engine.SetValue(assignment.Key, assignment.Value);
            }

            _window = GetDomNode(window);
            _lexicals = LexicalEnvironment.NewObjectEnvironment(_engine, _window, _engine.ExecutionContext.LexicalEnvironment, true);
            _variables = LexicalEnvironment.NewObjectEnvironment(_engine, _engine.Global, null, false);

            this.AddConstructors(_window, typeof(INode).GetAssembly());
            this.AddConstructors(_window, this.GetType().GetAssembly());
            this.AddInstances(_window, this.GetType());
        }

        #endregion

        #region Properties

        public DomNodeInstance Window
        {
            get { return _window; }
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
            return _references.GetOrCreate(obj, CreateInstance);
        }

        public ObjectInstance GetDomPrototype(Type type)
        {
            return _prototypes.GetOrCreate(type, CreatePrototype);
        }

        public JsValue RunScript(String source, JsValue context)
        {
            _engine.EnterExecutionContext(Lexicals, Variables, context);
            _engine.Execute(source);
            _engine.LeaveExecutionContext();
            return _engine.GetCompletionValue();
        }

        #endregion

        #region Helpers

        private DomNodeInstance CreateInstance(Object obj)
        {
            return new DomNodeInstance(this, obj);
        }

        private ObjectInstance CreatePrototype(Type type)
        {
            return new DomPrototypeInstance(this, type);
        }

        #endregion
    }
}
