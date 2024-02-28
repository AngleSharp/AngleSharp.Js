namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    sealed class EngineInstance
    {
        #region Fields

        private readonly Engine _engine;
        private readonly PrototypeCache _prototypes;
        private readonly ReferenceCache _references;
        private readonly IEnumerable<Assembly> _libs;
        private readonly DomNodeInstance _window;

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments, IEnumerable<Assembly> libs)
        {
            _engine = new Engine();
            _prototypes = new PrototypeCache(_engine);
            _references = new ReferenceCache();
            _libs = libs;

            foreach (var assignment in assignments)
            {
                _engine.SetValue(assignment.Key, assignment.Value);
            }

            _window = GetDomNode(window);

            foreach (var lib in libs)
            {
                this.AddConstructors(_window, lib);
                this.AddInstances(_window, lib);
            }

            foreach (var property in _window.GetOwnProperties())
            {
                _engine.Global.FastSetProperty(property.Key.ToString(), property.Value);
            }

            foreach (var prototypeProperty in Window.Prototype.GetOwnProperties())
            {
                _engine.Global.FastSetProperty(prototypeProperty.Key.ToString(), prototypeProperty.Value);
            }

            _engine.Global.Prototype = _window.Prototype;
        }

        #endregion

        #region Properties

        public IEnumerable<Assembly> Libs => _libs;

        public DomNodeInstance Window => _window;

        public Engine Jint => _engine;

        #endregion

        #region Methods

        public DomNodeInstance GetDomNode(Object obj) => _references.GetOrCreate(obj, CreateInstance);

        public ObjectInstance GetDomPrototype(Type type) => _prototypes.GetOrCreate(type, CreatePrototype);

        public JsValue RunScript(String source, JsValue context)
        {
            lock (_engine)
            {
                return _engine.Evaluate(source);
            }
        }

        #endregion

        #region Helpers

        private DomNodeInstance CreateInstance(Object obj) => new DomNodeInstance(this, obj);

        private ObjectInstance CreatePrototype(Type type) => new DomPrototypeInstance(this, type);

        #endregion
    }
}
