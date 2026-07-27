namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Io;
    using AngleSharp.Js.Cache;
    using AngleSharp.Text;
    using Jint;
    using Jint.Native;
    using Jint.Native.Json;
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
            _libs = libs;
            _prototypes = new PrototypeCache(_engine, libs);
            _references = new ReferenceCache();

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

        /// <summary>
        /// Gets the constructor object of the given type, building it on first ask. The
        /// prototype keeps it, so that naming the type and reading "constructor" off one of
        /// its instances arrive at the same object.
        /// </summary>
        public DomConstructorInstance GetDomConstructor(ConstructorDefinition definition)
        {
            //  Only the prototype of System.Object is not one of ours, and that type is not
            //  exposed as a constructor, so it never reaches this point.
            var prototype = (DomPrototypeInstance)GetDomPrototype(definition.Type);
            return prototype.GetConstructor(definition);
        }

        public JsValue RunScript(String source, String type, String sourceUrl)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = MimeTypeNames.DefaultJavaScript;
            }

            lock (_engine)
            {
                if (MimeTypeNames.IsJavaScript(type))
                {
                    var prepared = ScriptCache.GetOrCreate(source);
                    //  An invalid result means the source did not parse; hand it to the
                    //  engine as text so the syntax error is reported as usual.
                    return prepared.IsValid ? _engine.Evaluate(prepared) : _engine.Evaluate(source);
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

        private DomNodeInstance CreateInstance(Object obj) => new DomNodeInstance(this, obj);

        private ObjectInstance CreatePrototype(Type type) => new DomPrototypeInstance(this, type);

        #endregion
    }
}
