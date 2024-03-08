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
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;

    sealed class EngineInstance
    {
        #region Fields

        private readonly Engine _engine;
        private readonly PrototypeCache _prototypes;
        private readonly ReferenceCache _references;
        private readonly IEnumerable<Assembly> _libs;
        private readonly DomNodeInstance _window;
        private readonly IResourceLoader _resourceLoader;
        private readonly IElement _scriptElement;
        private readonly string _documentUrl;

        #endregion

        #region ctor

        public EngineInstance(IWindow window, IDictionary<String, Object> assignments, IEnumerable<Assembly> libs)
        {
            _resourceLoader = window.Document.Context.GetService<IResourceLoader>();

            _scriptElement = window.Document.CreateElement(TagNames.Script);

            _documentUrl = window.Document.Url;

            _engine = new Engine((options) =>
            {
                options.EnableModules(new JsModuleLoader(this, _documentUrl, false));
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

        public JsValue RunScript(String source, String type, JsValue context)
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
                    var specifier = Guid.NewGuid().ToString();

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
            JsImportMap importMap;

            try
            {
                importMap = JsonSerializer.Deserialize<JsImportMap>(source);
            }
            catch (JsonException)
            {
                importMap = null;
            }

            // get list of imports based on any scoped imports for the current document path, and any global imports
            var imports = new Dictionary<string, Uri>();
            var documentPathName = Url.Create(_documentUrl).PathName.ToLower();

            if (importMap?.Scopes?.Count > 0)
            {
                var scopePaths = importMap.Scopes.Keys.OrderByDescending(k => k.Length);

                foreach (var scopePath in scopePaths)
                {
                    if (!documentPathName.Contains(scopePath.ToLower()))
                    {
                        continue;
                    }

                    var scopeImports = importMap.Scopes[scopePath];

                    foreach (var scopeImport in scopeImports)
                    {
                        if (!imports.ContainsKey(scopeImport.Key))
                        {
                            imports.Add(scopeImport.Key, scopeImport.Value);
                        }
                    }
                }
            }

            if (importMap?.Imports?.Count > 0)
            {
                foreach (var globalImport in importMap.Imports)
                {
                    if (!imports.ContainsKey(globalImport.Key))
                    {
                        imports.Add(globalImport.Key, globalImport.Value);
                    }
                }
            }

            foreach (var import in imports)
            {
                var moduleContent = FetchModule(import.Value);

                ImportModule(import.Key, moduleContent);
            }

            return JsValue.Undefined;
        }

        private JsValue ImportModule(String specifier, String source)
        {
            _engine.Modules.Add(specifier, source);
            _engine.Modules.Import(specifier);

            return JsValue.Undefined;
        }

        public string FetchModule(Uri moduleUrl)
        {
            if (_resourceLoader == null)
            {
                return string.Empty;
            }

            if (!moduleUrl.IsAbsoluteUri)
            {
                moduleUrl = new Uri(new Uri(_documentUrl), moduleUrl);
            }

            var importUrl = Url.Convert(moduleUrl);

            var request = new ResourceRequest(_scriptElement, importUrl);

            var response = _resourceLoader.FetchAsync(request).Task.Result;

            string content;

            using (var streamReader = new StreamReader(response.Content))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }

        #endregion

        #region Helpers

        private DomNodeInstance CreateInstance(Object obj) => new DomNodeInstance(this, obj);

        private ObjectInstance CreatePrototype(Type type) => new DomPrototypeInstance(this, type);

        #endregion
    }
}
