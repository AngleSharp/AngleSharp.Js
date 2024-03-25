namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Text;
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
        private readonly JsImportMap _importMap;

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments, IEnumerable<Assembly> libs)
        {
            _importMap = new JsImportMap();

            _engine = new Engine((options) =>
            {
                options.EnableModules(new JsModuleLoader(this, window.Document, false));
            });
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
                this.AddConstructorFunctions(_window, lib);
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

        public JsImportMap ImportMap => _importMap;

        #endregion

        #region Methods

        public DomNodeInstance GetDomNode(Object obj) => _references.GetOrCreate(obj, CreateInstance);

        public ObjectInstance GetDomPrototype(Type type) => _prototypes.GetOrCreate(type, CreatePrototype);

        public JsValue RunScript(String source, String type, String sourceUrl, JsValue context)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = MimeTypeNames.DefaultJavaScript;
            }

            lock (_engine)
            {
                if (MimeTypeNames.IsJavaScript(type))
                {
                    return _engine.Evaluate(source);
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
            var importMap = _engine.Evaluate($"JSON.parse('{source}')").AsObject();

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

        private DomNodeInstance CreateInstance(Object obj) => new DomNodeInstance(this, obj);

        private ObjectInstance CreatePrototype(Type type) => new DomPrototypeInstance(this, type);

        #endregion
    }
}
